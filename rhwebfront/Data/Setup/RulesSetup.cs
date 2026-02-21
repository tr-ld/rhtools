using Microsoft.EntityFrameworkCore;
using rhdata.Rules;

namespace RHWebFront.Data.Setup;

public static class RulesSetup
{
    extension(ModelBuilder modelBuilder)
    {
        public void SetupRules()
        {
            modelBuilder.SeedRules();
            modelBuilder.ConfigureRulesTable();
        }

        public void SeedRules()
        {
            // TriggerTemplate seed
            modelBuilder.Entity<TriggerTemplate>().HasData(
                new TriggerTemplate { Id = 1, Name = "DownPercent", Description = "Triggers when price decreases by a percentage" },
                new TriggerTemplate { Id = 2, Name = "UpPercent", Description = "Triggers when price increases by a percentage" },
                new TriggerTemplate { Id = 3, Name = "DownAbsolute", Description = "Triggers when price decreases by an absolute amount" },
                new TriggerTemplate { Id = 4, Name = "UpAbsolute", Description = "Triggers when price increases by an absolute amount" }
            );

            // PrecisionTemplate seed
            modelBuilder.Entity<PrecisionTemplate>().HasData(
                new PrecisionTemplate { Id = 1, Name = "Seconds", Description = "Evaluate rule every N seconds" },
                new PrecisionTemplate { Id = 2, Name = "Minutes", Description = "Evaluate rule every N minutes" },
                new PrecisionTemplate { Id = 3, Name = "Hours", Description = "Evaluate rule every N hours" },
                new PrecisionTemplate { Id = 4, Name = "Days", Description = "Evaluate rule every N days" }
            );

            // AmountTemplate seed
            modelBuilder.Entity<AmountTemplate>().HasData(
                new AmountTemplate { Id = 1, Name = "Absolute", Description = "Specific quantity of the asset" },
                new AmountTemplate { Id = 2, Name = "Percent", Description = "Percentage of available holdings" },
                new AmountTemplate { Id = 3, Name = "All", Description = "All available holdings" }
            );

            // ActionTemplate seed
            modelBuilder.Entity<ActionTemplate>().HasData(
                new ActionTemplate { Id = 1, Name = "LimitSellAbsolute", Description = "Limit sell order at absolute price" },
                new ActionTemplate { Id = 2, Name = "LimitSellRelativeAtCreate", Description = "Limit sell order at price relative to rule creation" },
                new ActionTemplate { Id = 3, Name = "LimitSellRelativeAtExecute", Description = "Limit sell order at price relative to trigger execution" },
                new ActionTemplate { Id = 4, Name = "LimitBuyAbsolute", Description = "Limit buy order at absolute price" },
                new ActionTemplate { Id = 5, Name = "LimitBuyRelativeAtCreate", Description = "Limit buy order at price relative to rule creation" },
                new ActionTemplate { Id = 6, Name = "LimitBuyRelativeAtExecute", Description = "Limit buy order at price relative to trigger execution" },
                new ActionTemplate { Id = 7, Name = "MarketSell", Description = "Market sell order executed immediately at current market price" },
                new ActionTemplate { Id = 8, Name = "MarketBuy", Description = "Market buy order executed immediately at current market price" }
            );
        }

        public void ConfigureRulesTable()
        {
            modelBuilder.Entity<TriggerTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RuleTrigger>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<PrecisionTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RulePrecision>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<AmountTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RuleAmount>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<ActionTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RuleAction>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RuleSet>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RuleOrderPosition>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });
        }
    }
}