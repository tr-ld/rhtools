using Microsoft.EntityFrameworkCore;
using rhdata;

namespace RHWebFront.Data.Setup;

public static class WatchlistSetup
{
    extension(ModelBuilder modelBuilder)
    {
        public void SetupWatchlist()
        {
            modelBuilder.ConfigureWatchlistTable();
            modelBuilder.SeedWatchlistData();
        }

        public void ConfigureWatchlistTable()
        {
            modelBuilder.Entity<SymbolWatchlistEntry>(entity =>
            {
                entity.HasIndex(e => new { e.Symbol, e.Currency }).IsUnique();
                entity.HasIndex(e => e.IsActive);

                entity.Property(e => e.Currency).HasDefaultValue("USD");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });
        }

        public void SeedWatchlistData()
        {
            modelBuilder.Entity<SymbolWatchlistEntry>().HasData(
                new SymbolWatchlistEntry { Id = 1, Symbol = "BTC", IsActive = true },
                new SymbolWatchlistEntry { Id = 2, Symbol = "ETH", IsActive = true },
                new SymbolWatchlistEntry { Id = 3, Symbol = "DOGE", IsActive = true },
                new SymbolWatchlistEntry { Id = 4, Symbol = "SHIB", IsActive = true },
                new SymbolWatchlistEntry { Id = 5, Symbol = "SOL", IsActive = true },
                new SymbolWatchlistEntry { Id = 6, Symbol = "XRP", IsActive = true }
            );
        }
    }
}