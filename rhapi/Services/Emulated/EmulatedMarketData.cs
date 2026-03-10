using System;
using System.Collections.Generic;
using rhapi.Enums;
using rhapi.Poco;
using rhapi.Poco.Market;
using rhapi.Poco.Order;
using rhapi.Poco.Order.OrderConfig;

namespace rhapi.Services.Emulated;

internal static class EmulatedMarketData
{
    internal const string ACCOUNT_NUMBER = "EMULATED123456";

    internal static readonly HashSet<string> RequiredSymbols = ["BTC", "ETH", "DOGE", "SOL", "XRP", "SHIB"];

    internal static List<RhHolding> CreateInitialHoldings() =>
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

    internal static List<RhOrder> CreateInitialOrders() =>
    [
        // Open orders
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "BTC-USD", Side = Side.buy, Type = OrderType.limit, State = OrderState.open, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddHours(-2), UpdatedAt = DateTime.UtcNow.AddHours(-2), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 0.01m, LimitPrice = 51000.00m } },
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "ETH-USD", Side = Side.sell, Type = OrderType.limit, State = OrderState.open, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddHours(-1), UpdatedAt = DateTime.UtcNow.AddHours(-1), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 0.5m, LimitPrice = 2850.00m } },
        
        // Partially filled order
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "SOL-USD", 
            Side = Side.buy, 
            Type = OrderType.limit, 
            State = OrderState.partially_filled, 
            AveragePrice = 109.50m, 
            FilledAssetQuantity = 2.5m, 
            CreatedAt = DateTime.UtcNow.AddHours(-3), 
            UpdatedAt = DateTime.UtcNow.AddMinutes(-30), 
            LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 5m, LimitPrice = 110.00m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 109.45m, Quantity = 1.5m, TimeStamp = DateTime.UtcNow.AddHours(-3).AddMinutes(1) },
                new RhOrderExecution { EffectivePrice = 109.55m, Quantity = 1.0m, TimeStamp = DateTime.UtcNow.AddHours(-3).AddMinutes(5) }
            ]
        },
        
        // Filled orders
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "BTC-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 51800.00m, 
            FilledAssetQuantity = 0.02m, 
            CreatedAt = DateTime.UtcNow.AddDays(-1), 
            UpdatedAt = DateTime.UtcNow.AddDays(-1), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 0.02m },
            Executions = [new RhOrderExecution { EffectivePrice = 51800.00m, Quantity = 0.02m, TimeStamp = DateTime.UtcNow.AddDays(-1).AddSeconds(2) }]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "ETH-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 2795.00m, 
            FilledAssetQuantity = 1.0m, 
            CreatedAt = DateTime.UtcNow.AddDays(-2), 
            UpdatedAt = DateTime.UtcNow.AddDays(-2), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 1.0m },
            Executions = [new RhOrderExecution { EffectivePrice = 2795.00m, Quantity = 1.0m, TimeStamp = DateTime.UtcNow.AddDays(-2).AddSeconds(1) }]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "DOGE-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 0.148m, 
            FilledAssetQuantity = 1000m, 
            CreatedAt = DateTime.UtcNow.AddDays(-3), 
            UpdatedAt = DateTime.UtcNow.AddDays(-3), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 1000m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 0.148m, Quantity = 600m, TimeStamp = DateTime.UtcNow.AddDays(-3).AddSeconds(1) },
                new RhOrderExecution { EffectivePrice = 0.148m, Quantity = 400m, TimeStamp = DateTime.UtcNow.AddDays(-3).AddSeconds(2) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "XRP-USD", 
            Side = Side.sell, 
            Type = OrderType.limit, 
            State = OrderState.filled, 
            AveragePrice = 0.52m, 
            FilledAssetQuantity = 500m, 
            CreatedAt = DateTime.UtcNow.AddDays(-4), 
            UpdatedAt = DateTime.UtcNow.AddDays(-4), 
            LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 500m, LimitPrice = 0.52m },
            Executions = [new RhOrderExecution { EffectivePrice = 0.52m, Quantity = 500m, TimeStamp = DateTime.UtcNow.AddDays(-4).AddSeconds(3) }]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "SHIB-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 0.00001200m, 
            FilledAssetQuantity = 100000m, 
            CreatedAt = DateTime.UtcNow.AddDays(-5), 
            UpdatedAt = DateTime.UtcNow.AddDays(-5), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 100000m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 0.00001200m, Quantity = 50000m, TimeStamp = DateTime.UtcNow.AddDays(-5).AddSeconds(1) },
                new RhOrderExecution { EffectivePrice = 0.00001200m, Quantity = 50000m, TimeStamp = DateTime.UtcNow.AddDays(-5).AddSeconds(2) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "SOL-USD", 
            Side = Side.buy, 
            Type = OrderType.limit, 
            State = OrderState.filled, 
            AveragePrice = 108.75m, 
            FilledAssetQuantity = 3.0m, 
            CreatedAt = DateTime.UtcNow.AddDays(-6), 
            UpdatedAt = DateTime.UtcNow.AddDays(-6), 
            LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 3.0m, LimitPrice = 109.00m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 108.70m, Quantity = 1.0m, TimeStamp = DateTime.UtcNow.AddDays(-6).AddMinutes(1) },
                new RhOrderExecution { EffectivePrice = 108.75m, Quantity = 1.5m, TimeStamp = DateTime.UtcNow.AddDays(-6).AddMinutes(3) },
                new RhOrderExecution { EffectivePrice = 108.80m, Quantity = 0.5m, TimeStamp = DateTime.UtcNow.AddDays(-6).AddMinutes(5) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "BTC-USD", 
            Side = Side.sell, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 52500.00m, 
            FilledAssetQuantity = 0.015m, 
            CreatedAt = DateTime.UtcNow.AddDays(-7), 
            UpdatedAt = DateTime.UtcNow.AddDays(-7), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 0.015m },
            Executions = [new RhOrderExecution { EffectivePrice = 52500.00m, Quantity = 0.015m, TimeStamp = DateTime.UtcNow.AddDays(-7).AddSeconds(1) }]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "ETH-USD", 
            Side = Side.sell, 
            Type = OrderType.limit, 
            State = OrderState.filled, 
            AveragePrice = 2810.00m, 
            FilledAssetQuantity = 0.75m, 
            CreatedAt = DateTime.UtcNow.AddDays(-8), 
            UpdatedAt = DateTime.UtcNow.AddDays(-8), 
            LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 0.75m, LimitPrice = 2810.00m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 2810.00m, Quantity = 0.5m, TimeStamp = DateTime.UtcNow.AddDays(-8).AddMinutes(2) },
                new RhOrderExecution { EffectivePrice = 2810.00m, Quantity = 0.25m, TimeStamp = DateTime.UtcNow.AddDays(-8).AddMinutes(4) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "DOGE-USD", 
            Side = Side.sell, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 0.152m, 
            FilledAssetQuantity = 750m, 
            CreatedAt = DateTime.UtcNow.AddDays(-9), 
            UpdatedAt = DateTime.UtcNow.AddDays(-9), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 750m },
            Executions = [new RhOrderExecution { EffectivePrice = 0.152m, Quantity = 750m, TimeStamp = DateTime.UtcNow.AddDays(-9).AddSeconds(2) }]
        },
        
        // Canceled orders
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "BTC-USD", Side = Side.buy, Type = OrderType.limit, State = OrderState.canceled, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddDays(-10), UpdatedAt = DateTime.UtcNow.AddDays(-10).AddMinutes(15), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 0.03m, LimitPrice = 49000.00m } },
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "ETH-USD", Side = Side.sell, Type = OrderType.limit, State = OrderState.canceled, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddDays(-11), UpdatedAt = DateTime.UtcNow.AddDays(-11).AddMinutes(20), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 1.5m, LimitPrice = 2900.00m } },
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "XRP-USD", Side = Side.buy, Type = OrderType.limit, State = OrderState.canceled, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddDays(-12), UpdatedAt = DateTime.UtcNow.AddDays(-12).AddMinutes(10), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 1000m, LimitPrice = 0.50m } },
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "SOL-USD", Side = Side.sell, Type = OrderType.limit, State = OrderState.canceled, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddDays(-13), UpdatedAt = DateTime.UtcNow.AddDays(-13).AddMinutes(5), LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 5m, LimitPrice = 115.00m } },
        
        // Failed order
        new RhOrder { Id = Guid.NewGuid().ToString(), AccountNumber = ACCOUNT_NUMBER, ClientOrderId = Guid.NewGuid(), Symbol = "SHIB-USD", Side = Side.buy, Type = OrderType.market, State = OrderState.failed, AveragePrice = null, FilledAssetQuantity = 0m, CreatedAt = DateTime.UtcNow.AddDays(-14), UpdatedAt = DateTime.UtcNow.AddDays(-14), MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 500000m } },
        
        // More filled orders
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "DOGE-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 0.145m, 
            FilledAssetQuantity = 2000m, 
            CreatedAt = DateTime.UtcNow.AddDays(-15), 
            UpdatedAt = DateTime.UtcNow.AddDays(-15), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 2000m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 0.145m, Quantity = 1200m, TimeStamp = DateTime.UtcNow.AddDays(-15).AddSeconds(1) },
                new RhOrderExecution { EffectivePrice = 0.145m, Quantity = 800m, TimeStamp = DateTime.UtcNow.AddDays(-15).AddSeconds(2) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "BTC-USD", 
            Side = Side.buy, 
            Type = OrderType.limit, 
            State = OrderState.filled, 
            AveragePrice = 50500.00m, 
            FilledAssetQuantity = 0.025m, 
            CreatedAt = DateTime.UtcNow.AddDays(-16), 
            UpdatedAt = DateTime.UtcNow.AddDays(-16), 
            LimitOrderConfig = new RhLimitOrderConfig { AssetQuantity = 0.025m, LimitPrice = 50500.00m },
            Executions = [new RhOrderExecution { EffectivePrice = 50500.00m, Quantity = 0.025m, TimeStamp = DateTime.UtcNow.AddDays(-16).AddSeconds(5) }]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "ETH-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 2780.00m, 
            FilledAssetQuantity = 1.25m, 
            CreatedAt = DateTime.UtcNow.AddDays(-17), 
            UpdatedAt = DateTime.UtcNow.AddDays(-17), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 1.25m },
            Executions = 
            [
                new RhOrderExecution { EffectivePrice = 2780.00m, Quantity = 1.0m, TimeStamp = DateTime.UtcNow.AddDays(-17).AddSeconds(1) },
                new RhOrderExecution { EffectivePrice = 2780.00m, Quantity = 0.25m, TimeStamp = DateTime.UtcNow.AddDays(-17).AddSeconds(2) }
            ]
        },
        new RhOrder 
        { 
            Id = Guid.NewGuid().ToString(), 
            AccountNumber = ACCOUNT_NUMBER, 
            ClientOrderId = Guid.NewGuid(), 
            Symbol = "XRP-USD", 
            Side = Side.buy, 
            Type = OrderType.market, 
            State = OrderState.filled, 
            AveragePrice = 0.51m, 
            FilledAssetQuantity = 800m, 
            CreatedAt = DateTime.UtcNow.AddDays(-18), 
            UpdatedAt = DateTime.UtcNow.AddDays(-18), 
            MarketOrderConfig = new RhMarketOrderConfig { AssetQuantity = 800m },
            Executions = [new RhOrderExecution { EffectivePrice = 0.51m, Quantity = 800m, TimeStamp = DateTime.UtcNow.AddDays(-18).AddSeconds(2) }]
        }
    ];

    internal static readonly List<TradingPair> AllTradingPairs =
    [
        // Required pairs
        new TradingPair { AssetCode = "BTC", QuoteCode = "USD", Symbol = "BTC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "5.0", MinOrderSize = "0.00001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ETH", QuoteCode = "USD", Symbol = "ETH-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "50.0", MinOrderSize = "0.0001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "DOGE", QuoteCode = "USD", Symbol = "DOGE-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SOL", QuoteCode = "USD", Symbol = "SOL-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "100.0", MinOrderSize = "0.001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "XRP", QuoteCode = "USD", Symbol = "XRP-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SHIB", QuoteCode = "USD", Symbol = "SHIB-USD", AssetIncrement = "1", QuoteIncrement = "0.00000001", MaxOrderSize = "100000000.0", MinOrderSize = "1000.0", Status = TradingStatus.tradable },
        
        // Additional popular pairs
        new TradingPair { AssetCode = "ADA", QuoteCode = "USD", Symbol = "ADA-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "AVAX", QuoteCode = "USD", Symbol = "AVAX-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "DOT", QuoteCode = "USD", Symbol = "DOT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "1000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "MATIC", QuoteCode = "USD", Symbol = "MATIC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "LINK", QuoteCode = "USD", Symbol = "LINK-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "1000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "UNI", QuoteCode = "USD", Symbol = "UNI-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "1000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "LTC", QuoteCode = "USD", Symbol = "LTC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "100.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ATOM", QuoteCode = "USD", Symbol = "ATOM-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "1000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ALGO", QuoteCode = "USD", Symbol = "ALGO-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        
        // Randomized additional pairs
        new TradingPair { AssetCode = "XLM", QuoteCode = "USD", Symbol = "XLM-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "20000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "AAVE", QuoteCode = "USD", Symbol = "AAVE-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "100.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "FIL", QuoteCode = "USD", Symbol = "FIL-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "VET", QuoteCode = "USD", Symbol = "VET-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "100.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ETC", QuoteCode = "USD", Symbol = "ETC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "TRX", QuoteCode = "USD", Symbol = "TRX-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "XTZ", QuoteCode = "USD", Symbol = "XTZ-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "HBAR", QuoteCode = "USD", Symbol = "HBAR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ICP", QuoteCode = "USD", Symbol = "ICP-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "APT", QuoteCode = "USD", Symbol = "APT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "NEAR", QuoteCode = "USD", Symbol = "NEAR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "FTM", QuoteCode = "USD", Symbol = "FTM-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SAND", QuoteCode = "USD", Symbol = "SAND-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "MANA", QuoteCode = "USD", Symbol = "MANA-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "AXS", QuoteCode = "USD", Symbol = "AXS-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "GALA", QuoteCode = "USD", Symbol = "GALA-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ENJ", QuoteCode = "USD", Symbol = "ENJ-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "CHZ", QuoteCode = "USD", Symbol = "CHZ-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "CRV", QuoteCode = "USD", Symbol = "CRV-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "COMP", QuoteCode = "USD", Symbol = "COMP-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "200.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "MKR", QuoteCode = "USD", Symbol = "MKR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "10.0", MinOrderSize = "0.001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SNX", QuoteCode = "USD", Symbol = "SNX-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SUSHI", QuoteCode = "USD", Symbol = "SUSHI-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "YFI", QuoteCode = "USD", Symbol = "YFI-USD", AssetIncrement = "0.00000001", QuoteIncrement = "1.00", MaxOrderSize = "5.0", MinOrderSize = "0.0001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "BAT", QuoteCode = "USD", Symbol = "BAT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "20000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ZRX", QuoteCode = "USD", Symbol = "ZRX-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "20000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "1INCH", QuoteCode = "USD", Symbol = "1INCH-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "GRT", QuoteCode = "USD", Symbol = "GRT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "LRC", QuoteCode = "USD", Symbol = "LRC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "20000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SKL", QuoteCode = "USD", Symbol = "SKL-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "OMG", QuoteCode = "USD", Symbol = "OMG-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "REN", QuoteCode = "USD", Symbol = "REN-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "NKN", QuoteCode = "USD", Symbol = "NKN-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "BCH", QuoteCode = "USD", Symbol = "BCH-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "50.0", MinOrderSize = "0.001", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "EOS", QuoteCode = "USD", Symbol = "EOS-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "XMR", QuoteCode = "USD", Symbol = "XMR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "100.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ZEC", QuoteCode = "USD", Symbol = "ZEC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "200.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "DASH", QuoteCode = "USD", Symbol = "DASH-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "200.0", MinOrderSize = "0.01", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "QTUM", QuoteCode = "USD", Symbol = "QTUM-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ZIL", QuoteCode = "USD", Symbol = "ZIL-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "100000.0", MinOrderSize = "100.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ICX", QuoteCode = "USD", Symbol = "ICX-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ONT", QuoteCode = "USD", Symbol = "ONT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "WAVES", QuoteCode = "USD", Symbol = "WAVES-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "BTT", QuoteCode = "USD", Symbol = "BTT-USD", AssetIncrement = "1", QuoteIncrement = "0.000000001", MaxOrderSize = "1000000000.0", MinOrderSize = "10000.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "SC", QuoteCode = "USD", Symbol = "SC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.000001", MaxOrderSize = "500000.0", MinOrderSize = "100.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "LSK", QuoteCode = "USD", Symbol = "LSK-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ARK", QuoteCode = "USD", Symbol = "ARK-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "5000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "KNC", QuoteCode = "USD", Symbol = "KNC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "ANKR", QuoteCode = "USD", Symbol = "ANKR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "100000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "BAND", QuoteCode = "USD", Symbol = "BAND-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "2000.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "STORJ", QuoteCode = "USD", Symbol = "STORJ-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "CVC", QuoteCode = "USD", Symbol = "CVC-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.00001", MaxOrderSize = "50000.0", MinOrderSize = "10.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "NMR", QuoteCode = "USD", Symbol = "NMR-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "BNT", QuoteCode = "USD", Symbol = "BNT-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.0001", MaxOrderSize = "10000.0", MinOrderSize = "1.0", Status = TradingStatus.tradable },
        new TradingPair { AssetCode = "REP", QuoteCode = "USD", Symbol = "REP-USD", AssetIncrement = "0.00000001", QuoteIncrement = "0.01", MaxOrderSize = "500.0", MinOrderSize = "0.1", Status = TradingStatus.tradable }
    ];
}