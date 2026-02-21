using Microsoft.EntityFrameworkCore;
using rhdata;

namespace RHWebFront.Data.Setup
{
    public static class BidAskSetup
    {
        extension(ModelBuilder modelBuilder)
        {
            public void SetupBidAsks() { modelBuilder.ConfigureBidAsks(); }

            public void ConfigureBidAsks()
            {
                modelBuilder.Entity<BidAskHistoryEntry>(entity =>
                {
                    entity.HasIndex(e => e.Symbol);
                    entity.HasIndex(e => e.Timestamp);
                    entity.HasIndex(e => new { e.Symbol, e.Timestamp }).IsDescending(false, true);
                });
            }
        }
    }
}
