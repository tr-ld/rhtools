using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RHWebFront.Config;
using RHWebFront.Constants;
using RHWebFront.Repositories;
using rhdata;
using RHWebFront.Models;

namespace RHWebFront.Services;

public class BidAskPollingService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ISymbolWatchlistService _watchlistService;
    private readonly IBidAskNotificationService _notificationService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<BidAskPollingService> _logger;
    private readonly IOptionsMonitor<AppConfig> _appConfig;

    private PeriodicTimer _timer;
    private readonly SemaphoreSlim _restartLock = new(1, 1);
    private CancellationTokenSource _loopCts;

    public bool IsPolling { get; private set; }

    public BidAskPollingService(IServiceScopeFactory scopeFactory, ISymbolWatchlistService watchlistService, IBidAskNotificationService notificationService,
                                IMemoryCache cache, ILogger<BidAskPollingService> logger, IOptionsMonitor<AppConfig> appConfig)
    {
        _scopeFactory = scopeFactory;
        _watchlistService = watchlistService;
        _notificationService = notificationService;
        _cache = cache;
        _logger = logger;
        _appConfig = appConfig;

        _watchlistService.WatchlistChanged += OnWatchlistChanged;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BidAsk polling service starting");
        await StartPollingLoopAsync(stoppingToken);
    }

    private async Task StartPollingLoopAsync(CancellationToken stoppingToken)
    {
        await _restartLock.WaitAsync(stoppingToken);
        try
        {
            _loopCts?.Cancel();
            _loopCts?.Dispose();
            _loopCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

            _timer?.Dispose();
            var cadenceSeconds = _appConfig.CurrentValue.SelectedCadence;
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(cadenceSeconds));

            _logger.LogInformation("Starting polling loop with cadence of {Cadence}s", cadenceSeconds);
            IsPolling = true;

            _ = PollAsync(_loopCts.Token);
        }
        finally { _restartLock.Release(); }
    }

    private async Task PollAsync(CancellationToken ct)
    {
        // Execute immediately on start
        await ExecutePollIterationAsync(ct);

        // Then execute on timer
        while (!ct.IsCancellationRequested && _timer is not null)
        {
            try
            {
                await _timer.WaitForNextTickAsync(ct);
                await ExecutePollIterationAsync(ct);
            }
            catch (OperationCanceledException) { _logger.LogInformation("Polling loop cancelled"); break; }
        }

        IsPolling = false;
    }

    private async Task ExecutePollIterationAsync(CancellationToken ct)
    {
        try
        {
            var symbols = await _watchlistService.GetActiveSymbolsAsync();
            if (symbols.Length == 0) { _logger.LogDebug("No active symbols to poll"); return; }

            using var scope = _scopeFactory.CreateScope();
            var assetManager = scope.ServiceProvider.GetRequiredService<IRhAssetManager>();
            var repo = scope.ServiceProvider.GetRequiredService<IBidAskHistoryRepository>();

            _logger.LogDebug("Polling {Count} symbols: {Symbols}", symbols.Length, string.Join(", ", symbols));

            RHBidAsk[] bidAsks;
            try { bidAsks = await assetManager.GetBestBidAsk(new Dictionary<string, string[]> { ["symbol"] = symbols }); }
            catch (Exception ex) { _logger.LogWarning(ex, "Failed to retrieve bid/asks from API - continuing"); return; }

            if (bidAsks.Length == 0) { _logger.LogDebug("No bid/ask data returned"); return; }

            var now = DateTime.UtcNow;
            var historyEntries = PrepareBidAskEntryList(bidAsks);

            // Notify batch received (skeleton - nothing subscribes yet)
            _notificationService.NotifyBidAskReceived(bidAsks, now);

            try
            {
                await repo.AddRangeAsync(historyEntries, ct);
                _logger.LogDebug("Saved {Count} bid/ask entries to database", historyEntries.Count);
            }
            catch (Exception ex) { _logger.LogError(ex, "Failed to save bid/ask history to database"); }
        }
        catch (Exception ex) { _logger.LogError(ex, "Error in poll iteration"); }
    }

    private List<BidAskHistoryEntry> PrepareBidAskEntryList(RHBidAsk[] bidAsks)
    {
        var historyEntries = new List<BidAskHistoryEntry>();

        foreach (var bidAsk in bidAsks)
        {
            // Cache latest bid/ask (no TTL)
            _cache.Set(CacheKeys.BidAsk(bidAsk.Symbol), bidAsk, new MemoryCacheEntryOptions { Priority = CacheItemPriority.Normal });

            historyEntries.Add(new BidAskHistoryEntry
            {
                Symbol = bidAsk.Symbol,
                Price = bidAsk.Price,
                SellSpread = bidAsk.SellSpread,
                BuySpread = bidAsk.BuySpread,
                Timestamp = bidAsk.Timestamp
            });
        }

        return historyEntries;
    }

    private async void OnWatchlistChanged(object sender, WatchlistChangedEventArgs e)
    {
        _logger.LogInformation("Symbol list changed - restarting polling loop");
        await StartPollingLoopAsync(_loopCts?.Token ?? CancellationToken.None);
    }

    public override void Dispose()
    {
        _watchlistService.WatchlistChanged -= OnWatchlistChanged;
        _timer?.Dispose();
        _loopCts?.Dispose();
        _restartLock.Dispose();

        base.Dispose();
    }
}