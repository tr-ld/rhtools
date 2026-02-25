using Microsoft.EntityFrameworkCore;
using rhdata;
using rhdata.Rules;
using RHWebFront.Data.Setup;

namespace RHWebFront.Data;

public class RhDbContext(DbContextOptions<RhDbContext> options) : DbContext(options)
{
    public DbSet<BidAskHistoryEntry> BidAskHistory { get; set; }
    public DbSet<SymbolWatchlistEntry> SymbolWatchlist { get; set; }

    // Rule-related DbSets
    public DbSet<RuleSet> RuleSets { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<TriggerTemplate> TriggerTemplates { get; set; }
    public DbSet<RuleTrigger> RuleTriggers { get; set; }
    public DbSet<PrecisionTemplate> PrecisionTemplates { get; set; }
    public DbSet<RulePrecision> RulePrecisions { get; set; }
    public DbSet<AmountTemplate> AmountTemplates { get; set; }
    public DbSet<RuleAmount> RuleAmounts { get; set; }
    public DbSet<ActionTemplate> ActionTemplates { get; set; }
    public DbSet<RuleAction> RuleActions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.SetupBidAsks();
        modelBuilder.SetupWatchlist();
        modelBuilder.SetupRules();
    }
}