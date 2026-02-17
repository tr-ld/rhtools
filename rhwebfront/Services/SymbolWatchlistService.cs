using Microsoft.Extensions.Options;
using RHWebFront.Models;
using RHWebFront.Repositories;
using rhdata;
using RHWebFront.Config;

namespace RHWebFront.Services;

public class SymbolWatchlistService(IServiceScopeFactory scopeFactory, ILogger<SymbolWatchlistService> logger, IOptionsMonitor<AppConfig> appConfig) : ISymbolWatchlistService
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private string[] _cachedSymbols = [];

    public event EventHandler<WatchlistChangedEventArgs> WatchlistChanged;

    public async Task<string[]> GetActiveSymbolsAsync()
    {
        await _lock.WaitAsync();
        try
        {
            if (_cachedSymbols.Length > 0) return _cachedSymbols;

            await LoadSymbolsFromDbAsync();
            return _cachedSymbols;
        }
        finally { _lock.Release(); }
    }

    #region Add/Activate
    public async Task AddSymbolAsync(string symbol) { await AddSymbolsAsync([symbol]); }
    public async Task AddSymbolsAsync(string[] symbols)
    {
        await _lock.WaitAsync();
        try
        {
            using var scope = scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISymbolWatchlistRepository>();

            await AddOrActivateSymbolsAsync(symbols, repo);
            await LoadSymbolsFromDbAsync();

            RaiseSymbolsChanged();
        }
        finally { _lock.Release(); }
    }

    private async Task AddOrActivateSymbolsAsync(string[] symbols, ISymbolWatchlistRepository repo)
    {
        var currency = appConfig.CurrentValue.TradeCurrency;

        foreach (var symbol in symbols)
        {
            var existing = await repo.GetBySymbolAndCurrencyAsync(symbol, currency);
            if (existing is null)
            { 
                await repo.AddAsync(new SymbolWatchlistEntry { Symbol = symbol, IsActive = true, Currency = currency, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }); 
            }
            else if (!existing.IsActive)
            {
                existing.IsActive = true;
                existing.UpdatedAt = DateTime.UtcNow;
                await repo.UpdateAsync(existing);
            }
        }
    }
    #endregion

    #region Remove/Deactivate
    public async Task RemoveSymbolAsync(string symbol) { await RemoveSymbolsAsync([symbol]); }
    public async Task RemoveSymbolsAsync(string[] symbols)
    {
        await _lock.WaitAsync();
        try
        {
            using var scope = scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISymbolWatchlistRepository>();
            var currency = appConfig.CurrentValue.TradeCurrency;

            foreach (var symbol in symbols)
            {
                var existing = await repo.GetBySymbolAndCurrencyAsync(symbol, currency);
                if (existing is null || !existing.IsActive) continue;

                await DeactivateSymbolAsync(existing, repo);
            }

            await LoadSymbolsFromDbAsync();
            RaiseSymbolsChanged();
        }
        finally { _lock.Release(); }
    }

    private static async Task DeactivateSymbolAsync(SymbolWatchlistEntry entry, ISymbolWatchlistRepository repo)
    {
        entry.IsActive = false;
        entry.UpdatedAt = DateTime.UtcNow;
        await repo.UpdateAsync(entry);
    }
    #endregion

    public async Task ReplaceSymbolsAsync(string[] symbols)
    {
        await _lock.WaitAsync();
        try
        {
            using var scope = scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISymbolWatchlistRepository>();
            var currency = appConfig.CurrentValue.TradeCurrency;

            var allEntries = await repo.GetActiveSymbolsAsync(currency);
            var newSymbolSet = symbols.ToHashSet();

            foreach (var entry in allEntries)
            {
                if (newSymbolSet.Contains(entry.Symbol)) continue;

                await DeactivateSymbolAsync(entry, repo);
            }

            await AddOrActivateSymbolsAsync(symbols, repo);
            await LoadSymbolsFromDbAsync();

            RaiseSymbolsChanged();
        }
        finally { _lock.Release(); }
    }

    private async Task LoadSymbolsFromDbAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ISymbolWatchlistRepository>();
        
        var currency = appConfig.CurrentValue.TradeCurrency;
        var entries = await repo.GetActiveSymbolsAsync(currency);
        
        _cachedSymbols = entries.Select(e => e.Symbol).ToArray();
        logger.LogInformation("Loaded {Count} active symbols for currency {Currency} from database", _cachedSymbols.Length, currency);
    }

    private void RaiseSymbolsChanged() { WatchlistChanged?.Invoke(this, new WatchlistChangedEventArgs { Symbols = _cachedSymbols }); }
}