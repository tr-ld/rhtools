using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using rhapi.Enums;
using rhapi.Poco;
using rhapi.Poco.Market;
using rhapi.Poco.Order;
using rhapi.Poco.Order.OrderConfig;
using rhapi.Poco.Responses;

namespace rhapi.Services.Emulated;

public class EmulatedRhCryptoMarket : IRhCryptoMarket
{
    private readonly ILogger<EmulatedRhCryptoMarket> _logger;
    private readonly List<RhHolding> _holdings;
    private readonly List<RhOrder> _orders;

    public EmulatedRhCryptoMarket(ILogger<EmulatedRhCryptoMarket> logger)
    {
        _logger = logger;
        _holdings = EmulatedMarketData.CreateInitialHoldings();
        _orders = EmulatedMarketData.CreateInitialOrders();
        
        _logger.LogInformation("EmulatedRhCryptoMarket initialized with {OrderCount} orders, {HoldingCount} holdings, {TradingPairCount} trading pairs", 
            _orders.Count, _holdings.Count, EmulatedMarketData.AllTradingPairs.Count);
    }

    public Task<RhAccountResponse> GetAccount()
    {
        _logger.LogInformation("GetAccount called");
        
        return Task.FromResult(new RhAccountResponse
        {
            AccountNumber = EmulatedMarketData.ACCOUNT_NUMBER,
            Status = "active",
            BuyingPower = 10000.00m,
            BuyingPowerCurrency = "USD"
        });
    }

    public Task<RhHoldingsResponse> GetHoldings(string[] symbols)
    {
        _logger.LogInformation("GetHoldings called with symbols: {Symbols}", symbols != null ? string.Join(", ", symbols) : "all");
        
        var filtered = symbols?.Length > 0
            ? _holdings.Where(h => symbols.Contains(h.AssetCode)).ToArray()
            : _holdings.ToArray();

        return Task.FromResult(new RhHoldingsResponse { Results = filtered });
    }

    public Task<RhOrder> GetOrder(Guid orderId)
    {
        _logger.LogInformation("GetOrder called for order ID: {OrderId}", orderId);
        
        var order = _orders.FirstOrDefault(o => o.Id == orderId.ToString());
        
        if (order == null)
            _logger.LogWarning("Order not found: {OrderId}", orderId);
        
        return Task.FromResult(order);
    }

    public Task<RhOrdersResponse> GetOrders(RhOrderParams orderParams)
    {
        _logger.LogInformation("GetOrders called with symbol: {Symbol}, state: {State}", 
                               orderParams?.Symbol ?? "all", orderParams?.State?.ToString() ?? "all");
        
        var query = _orders.AsEnumerable();

        if (!string.IsNullOrEmpty(orderParams?.Symbol))
            query = query.Where(o => o.Symbol == orderParams.Symbol);

        if (orderParams?.State != null)
            query = query.Where(o => o.State == orderParams.State);

        return Task.FromResult(new RhOrdersResponse { Results = query.ToArray() });
    }

    public Task<RhOrder> PlaceOrder(RhPlaceOrderParams orderParams, IOptions<JsonOptions> options)
    {
        _logger.LogInformation("PlaceOrder called - Symbol: {Symbol}, Side: {Side}, Type: {Type}, Quantity: {Quantity}", 
            orderParams.Symbol, orderParams.Side, orderParams.Type, orderParams.CoalesceAssetQuantity());
        
        var orderId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var price = GetSimulatedPrice(orderParams.Symbol);

        var order = new RhOrder
        {
            Id = orderId.ToString(),
            AccountNumber = EmulatedMarketData.ACCOUNT_NUMBER,
            ClientOrderId = orderParams.ClientOrderId,
            Side = orderParams.Side,
            Symbol = orderParams.Symbol,
            Type = orderParams.Type,
            State = OrderState.filled,
            AveragePrice = price,
            FilledAssetQuantity = orderParams.CoalesceAssetQuantity(),
            CreatedAt = now,
            UpdatedAt = now,
            Executions = null
        };

        // Set the appropriate order config based on type
        if (orderParams.Type == OrderType.market)
        {
            order.MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = orderParams.CoalesceAssetQuantity() };
        }

        _orders.Add(order);
        _logger.LogInformation("Order placed successfully - OrderId: {OrderId}, Price: {Price}, Total orders: {OrderCount}", 
            orderId, price, _orders.Count);
        
        return Task.FromResult(order);
    }

    public Task<RhTradingPairsResponse> GetTradingPairs(RhTradingPairsParams pairParams)
    {
        var random = new Random();
        var results = new List<TradingPair>();

        // Always include required symbols
        var requiredPairs = EmulatedMarketData.AllTradingPairs.Where(tp => EmulatedMarketData.RequiredSymbols.Contains(tp.AssetCode)).ToList();
        results.AddRange(requiredPairs);

        // Get a random number of additional pairs (between 20 and all remaining pairs)
        var remainingPairs = EmulatedMarketData.AllTradingPairs.Where(tp => !EmulatedMarketData.RequiredSymbols.Contains(tp.AssetCode)).ToList();
        var randomCount = random.Next(20, remainingPairs.Count + 1);
        
        // Randomly select that many additional pairs
        var additionalPairs = remainingPairs.OrderBy(_ => random.Next()).Take(randomCount).ToList();
        results.AddRange(additionalPairs);

        _logger.LogInformation("GetTradingPairs returning {Count} pairs ({RequiredCount} required + {AdditionalCount} additional)", 
                               results.Count, requiredPairs.Count, additionalPairs.Count);

        return Task.FromResult(new RhTradingPairsResponse { Results = results.ToArray() });
    }

    public Task<RhEstimatedPriceResponse> GetEstimatedPrice(RhEstimatedPriceParams estParams)
    {
        _logger.LogInformation("GetEstimatedPrice called for symbol: {Symbol}, side: {Side}", estParams.Symbol, estParams.Side);
        
        var price = GetSimulatedPrice(estParams.Symbol);
        var spread = price * 0.001m; // 0.1% spread
        
        var results = new List<PriceEstimate>();

        if (estParams.Side is EstimateSide.bid or EstimateSide.both)
        {
            results.Add(new PriceEstimate
            {
                Symbol = estParams.Symbol,
                Side = EstimateSide.bid,
                Price = price,
                Quantity = 10.0m,
                BidInclusiveOfSellSpread = price - spread,
                SellSpread = spread,
                AskInclusiveOfBuySpread = price,
                BuySpread = 0m,
                Timestamp = DateTimeOffset.UtcNow
            });
        }

        if (estParams.Side is EstimateSide.ask or EstimateSide.both)
        {
            results.Add(new PriceEstimate
            {
                Symbol = estParams.Symbol,
                Side = EstimateSide.ask,
                Price = price,
                Quantity = 10.0m,
                BidInclusiveOfSellSpread = price,
                SellSpread = 0m,
                AskInclusiveOfBuySpread = price + spread,
                BuySpread = spread,
                Timestamp = DateTimeOffset.UtcNow
            });
        }

        return Task.FromResult(new RhEstimatedPriceResponse { Results = results.ToArray() });
    }

    public Task<RhBidAskResponse> GetBestBidAsk(RhBidAskParams bidAskParams)
    {
        _logger.LogInformation("GetBestBidAsk called for symbol: {Symbol}", bidAskParams.Symbol);
        
        // Split comma-separated symbols and create a result for each
        var symbols = bidAskParams.Symbol.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var results = new List<BidAskPrice>();

        foreach (var symbol in symbols)
        {
            var price = GetSimulatedPrice(symbol);
            var spread = price * 0.001m; // 0.1% spread

            results.Add(new BidAskPrice
            {
                Symbol = symbol,
                Price = price,
                Quantity = 10.0m,
                BidInclusiveOfSellSpread = price - spread,
                SellSpread = spread,
                AskInclusiveOfBuySpread = price + spread,
                BuySpread = spread,
                Timestamp = DateTimeOffset.UtcNow
            });
        }

        return Task.FromResult(new RhBidAskResponse { Results = results.ToArray() });
    }

    private static decimal GetSimulatedPrice(string symbol)
    {
        // Normalize symbol to include -USD if missing
        var normalizedSymbol = symbol.Contains('-') ? symbol : $"{symbol}-USD";
        
        return normalizedSymbol switch
        {
            "BTC-USD" => 52000.00m,
            "ETH-USD" => 2800.00m,
            "SOL-USD" => 110.00m,
            "XRP-USD" => 0.52m,
            "DOGE-USD" => 0.15m,
            "SHIB-USD" => 0.00001235m,
            "ADA-USD" => 0.45m,
            "AVAX-USD" => 38.00m,
            "DOT-USD" => 7.20m,
            "MATIC-USD" => 0.92m,
            "LINK-USD" => 15.50m,
            "UNI-USD" => 6.80m,
            "LTC-USD" => 95.00m,
            "ATOM-USD" => 10.50m,
            "ALGO-USD" => 0.18m,
            _ => 1.25m
        };
    }
}

internal static class EmulatedRhMarketExtensions
{
    internal static decimal CoalesceAssetQuantity(this RhPlaceOrderParams orderParams)
    {
        return orderParams.LimitOrderConfig.AssetQuantity
               ?? orderParams.MarketOrderConfig.AssetQuantity
               ?? orderParams.StopLimitOrderConfig.AssetQuantity
               ?? orderParams.StopLossOrderConfig.AssetQuantity
               ?? 0m;
    }
}