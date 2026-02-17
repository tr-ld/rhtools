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
            entity.HasIndex(e => new { e.Symbol, e.Currency }).IsUnique();
            entity.HasIndex(e => e.IsActive);
            
            entity.Property(e => e.Currency).HasDefaultValue("USD");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
        });
    }
}