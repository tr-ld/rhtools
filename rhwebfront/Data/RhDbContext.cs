using Microsoft.EntityFrameworkCore;
using rhdata;

namespace RHWebFront.Data;

public class RhDbContext(DbContextOptions<RhDbContext> options) : DbContext(options)
{
    public DbSet<BidAskHistoryEntry> BidAskHistory { get; set; }
    public DbSet<SymbolWatchlistEntry> SymbolWatchlist { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // BidAskHistoryEntry configuration
        modelBuilder.Entity<BidAskHistoryEntry>(entity =>
        {
            entity.HasIndex(e => e.Symbol);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => new { e.Symbol, e.Timestamp }).IsDescending(false, true);
        });

        // SymbolWatchlistEntry configuration
        modelBuilder.Entity<SymbolWatchlistEntry>(entity =>
        {
            entity.HasIndex(e => e.Symbol).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });
    }
}