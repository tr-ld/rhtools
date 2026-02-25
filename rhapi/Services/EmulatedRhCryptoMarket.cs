using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using rhapi.Enums;
using rhapi.Poco;
using rhapi.Poco.Market;
using rhapi.Poco.Order;
using rhapi.Poco.Order.OrderConfig;
using rhapi.Poco.Responses;

namespace rhapi.Services;

public class EmulatedRhCryptoMarket : IRhCryptoMarket
{
    private const string ACCOUNT_NUMBER = "EMULATED123456";
    
    private readonly List<RhHolding> _holdings =
    [
        new RhHolding
        {
            AccountNumber = ACCOUNT_NUMBER,
            AssetCode = "BTC",
            TotalQuantity = 0.5m,
            QuantityAvailableForTrading = 0.5m
        },
        new RhHolding
        {
            AccountNumber = ACCOUNT_NUMBER,
            AssetCode = "ETH",
            TotalQuantity = 2.0m,
            QuantityAvailableForTrading = 2.0m
        }
    ];

    private readonly List<RhOrder> _orders = [];
    private int _orderCounter = 1000;

    private readonly List<TradingPair> _tradingPairs =
    [
        new TradingPair
        {
            AssetCode = "BTC",
            QuoteCode = "USD",
            Symbol = "BTC-USD",
            AssetIncrement = "0.00000001",
            QuoteIncrement = "0.01",
            MaxOrderSize = "5.0",
            MinOrderSize = "0.00001",
            Status = TradingStatus.tradable
        },
        new TradingPair
        {
            AssetCode = "ETH",
            QuoteCode = "USD",
            Symbol = "ETH-USD",
            AssetIncrement = "0.00000001",
            QuoteIncrement = "0.01",
            MaxOrderSize = "50.0",
            MinOrderSize = "0.0001",
            Status = TradingStatus.tradable
        },
        new TradingPair
        {
            AssetCode = "DOGE",
            QuoteCode = "USD",
            Symbol = "DOGE-USD",
            AssetIncrement = "0.00000001",
            QuoteIncrement = "0.01",
            MaxOrderSize = "10000.0",
            MinOrderSize = "1.0",
            Status = TradingStatus.tradable
        }
    ];

    public Task<RhAccountResponse> GetAccount()
    {
        return Task.FromResult(new RhAccountResponse
        {
            AccountNumber = ACCOUNT_NUMBER,
            Status = "active",
            BuyingPower = 10000.00m,
            BuyingPowerCurrency = "USD"
        });
    }

    public Task<RhHoldingsResponse> GetHoldings(string[] symbols)
    {
        var filtered = symbols?.Length > 0
            ? _holdings.Where(h => symbols.Contains(h.AssetCode)).ToArray()
            : _holdings.ToArray();

        return Task.FromResult(new RhHoldingsResponse { Results = filtered });
    }

    public Task<RhOrder> GetOrder(Guid orderId)
    {
        var order = _orders.FirstOrDefault(o => o.Id == orderId.ToString());
        return Task.FromResult(order);
    }

    public Task<RhOrdersResponse> GetOrders(RhOrderParams orderParams)
    {
        var query = _orders.AsEnumerable();

        if (!string.IsNullOrEmpty(orderParams?.Symbol))
            query = query.Where(o => o.Symbol == orderParams.Symbol);

        if (orderParams?.State != null)
            query = query.Where(o => o.State == orderParams.State);

        return Task.FromResult(new RhOrdersResponse { Results = query.ToArray() });
    }

    public Task<RhOrder> PlaceOrder(RhPlaceOrderParams orderParams, IOptions<JsonOptions> options)
    {
        var orderId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var price = GetSimulatedPrice(orderParams.Symbol);

        var order = new RhOrder
        {
            Id = orderId.ToString(),
            AccountNumber = ACCOUNT_NUMBER,
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
        return Task.FromResult(order);
    }

    public Task<RhTradingPairsResponse> GetTradingPairs(RhTradingPairsParams pairParams)
    {
        var query = _tradingPairs.AsEnumerable();

        if (pairParams?.SymbolPairs is { Count: > 0 })
        {
            query = query.Where(tp => pairParams.SymbolPairs.Any(pair => 
                tp.AssetCode == pair.crypto && tp.QuoteCode == pair.currency));
        }

        return Task.FromResult(new RhTradingPairsResponse { Results = query.ToArray() });
    }

    public Task<RhEstimatedPriceResponse> GetEstimatedPrice(RhEstimatedPriceParams estParams)
    {
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
        var price = GetSimulatedPrice(bidAskParams.Symbol);
        var spread = price * 0.001m; // 0.1% spread

        var bidAsk = new BidAskPrice
        {
            Symbol = bidAskParams.Symbol,
            Price = price,
            Quantity = 10.0m,
            BidInclusiveOfSellSpread = price - spread,
            SellSpread = spread,
            AskInclusiveOfBuySpread = price + spread,
            BuySpread = spread,
            Timestamp = DateTimeOffset.UtcNow
        };

        return Task.FromResult(new RhBidAskResponse { Results = [bidAsk] });
    }

    private static decimal GetSimulatedPrice(string symbol)
    {
        return symbol switch
        {
            "BTC-USD" => 52000.00m,
            "ETH-USD" => 2800.00m,
            "DOGE-USD" => 0.15m,
            _ => 100.00m
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