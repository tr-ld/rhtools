using rhdata;

namespace RHWebFront.Models
{
    /// <summary>
    /// Aggregates all available data for a specific asset/symbol at a point in time.
    /// </summary>
    public class RHAssetSnapshot
    {
        /// <summary>
        /// The symbol this snapshot represents (e.g., "BTC-USD")
        /// </summary>
        public required string Symbol { get; init; }

        #region Core Data Sources
        /// <summary>
        /// Current holdings information for this asset, if owned
        /// </summary>
        public RHHolding? Holding { get; init; }

        /// <summary>
        /// Current best bid/ask pricing
        /// </summary>
        public RHBidAsk? BidAsk { get; init; }
        #endregion

        #region Computed Properties
        /// <summary>
        /// Current market price from bid/ask data
        /// </summary>
        public decimal? CurrentPrice => BidAsk?.Price;

        /// <summary>
        /// Total value of current position (quantity × current price)
        /// </summary>
        public decimal? PositionValue => Holding?.TotalQuantity * CurrentPrice;

        /// <summary>
        /// Whether the user currently holds this asset
        /// </summary>
        public bool HasPosition => Holding?.TotalQuantity > 0;
        #endregion

        #region Metadata
        /// <summary>
        /// When this snapshot was created/last updated
        /// </summary>
        public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Which data sources were successfully populated
        /// </summary>
        public DataAvailability Availability { get; init; }
        #endregion
    }

    [Flags]
    public enum DataAvailability
    {
        None = 0,
        Holding = 1 << 0,
        BidAsk = 1 << 1
    }
}