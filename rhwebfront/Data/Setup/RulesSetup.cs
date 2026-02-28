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
                new TriggerTemplate { Id = 1, Name = "Down Percent", Description = "Triggers when price decreases by a percentage" },
                new TriggerTemplate { Id = 2, Name = "Up Percent", Description = "Triggers when price increases by a percentage" },
                new TriggerTemplate { Id = 3, Name = "Down Flat", Description = "Triggers when price decreases by a flat amount" },
                new TriggerTemplate { Id = 4, Name = "Up Flat", Description = "Triggers when price increases by a flat amount" }
            );

            // PeriodicityTemplate seed
            modelBuilder.Entity<PeriodicityTemplate>().HasData(
                new PeriodicityTemplate { Id = 1, Name = "Seconds", Description = "Evaluate rule every N seconds" },
                new PeriodicityTemplate { Id = 2, Name = "Minutes", Description = "Evaluate rule every N minutes" },
                new PeriodicityTemplate { Id = 3, Name = "Hours", Description = "Evaluate rule every N hours" },
                new PeriodicityTemplate { Id = 4, Name = "Days", Description = "Evaluate rule every N days" }
            );

            // AmountTemplate seed
            modelBuilder.Entity<AmountTemplate>().HasData(
                new AmountTemplate { Id = 1, Name = "Flat", Description = "Specific quantity of the asset" },
                new AmountTemplate { Id = 2, Name = "Percent", Description = "Percentage of available holdings" },
                new AmountTemplate { Id = 3, Name = "Currency", Description = "Specific amount of holdings in currency" }
            );

            // PriceTemplate seed
            modelBuilder.Entity<PriceTemplate>().HasData(
                new PriceTemplate { Id = 1, Name = "Flat", Description = "Specific price point" },
                new PriceTemplate { Id = 2, Name = "Percent From Create", Description = "Percent offset from market price at rule activation" },
                new PriceTemplate { Id = 3, Name = "Percent From Execute", Description = "Percent offset from market price at trigger execution" }
            );

            // ActionTemplate seed
            modelBuilder.Entity<ActionTemplate>().HasData(
                new ActionTemplate { Id = 1, Name = "Limit Sell", Description = "Sell order at a specific price" },
                new ActionTemplate { Id = 2, Name = "Limit Buy", Description = "Buy order at a specific price" },
                new ActionTemplate { Id = 3, Name = "Market Sell", Description = "Sell order executed immediately at current market price" },
                new ActionTemplate { Id = 4, Name = "Market Buy", Description = "Buy order executed immediately at current market price" }
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

            modelBuilder.Entity<PeriodicityTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RulePeriodicity>(entity =>
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

            modelBuilder.Entity<PriceTemplate>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });

            modelBuilder.Entity<RulePrice>(entity =>
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

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("DATETIME('now')");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("DATETIME('now')");
            });
        }
    }
}