using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RHWebFront.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActionTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AmountTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmountTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BidAskHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    SellSpread = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    BuySpread = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidAskHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodicityTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodicityTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolWatchlist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false, defaultValue: "USD"),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolWatchlist", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TriggerTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TriggerTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActionTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleActions_ActionTemplates_ActionTemplateId",
                        column: x => x.ActionTemplateId,
                        principalTable: "ActionTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleAmounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleAmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleAmounts_AmountTemplates_AmountTemplateId",
                        column: x => x.AmountTemplateId,
                        principalTable: "AmountTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulePeriodicities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PeriodicityTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulePeriodicities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulePeriodicities_PeriodicityTemplates_PeriodicityTemplateId",
                        column: x => x.PeriodicityTemplateId,
                        principalTable: "PeriodicityTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RulePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PriceTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(38,18)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulePrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulePrices_PriceTemplates_PriceTemplateId",
                        column: x => x.PriceTemplateId,
                        principalTable: "PriceTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleTriggers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TriggerTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleTriggers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleTriggers_TriggerTemplates_TriggerTemplateId",
                        column: x => x.TriggerTemplateId,
                        principalTable: "TriggerTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RuleSetId = table.Column<int>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PeriodicityId = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountId = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_RuleActions_ActionId",
                        column: x => x.ActionId,
                        principalTable: "RuleActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RuleAmounts_AmountId",
                        column: x => x.AmountId,
                        principalTable: "RuleAmounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RulePeriodicities_PeriodicityId",
                        column: x => x.PeriodicityId,
                        principalTable: "RulePeriodicities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RulePrices_PriceId",
                        column: x => x.PriceId,
                        principalTable: "RulePrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RuleSets_RuleSetId",
                        column: x => x.RuleSetId,
                        principalTable: "RuleSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RuleTriggers_TriggerId",
                        column: x => x.TriggerId,
                        principalTable: "RuleTriggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ActionTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Sell order at a specific price", "Limit Sell" },
                    { 2, "Buy order at a specific price", "Limit Buy" },
                    { 3, "Sell order executed immediately at current market price", "Market Sell" },
                    { 4, "Buy order executed immediately at current market price", "Market Buy" }
                });

            migrationBuilder.InsertData(
                table: "AmountTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Specific quantity of the asset", "Flat" },
                    { 2, "Percentage of available holdings", "Percent" },
                    { 3, "Specific amount of holdings in currency", "Currency" }
                });

            migrationBuilder.InsertData(
                table: "PeriodicityTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Evaluate rule every N seconds", "Seconds" },
                    { 2, "Evaluate rule every N minutes", "Minutes" },
                    { 3, "Evaluate rule every N hours", "Hours" },
                    { 4, "Evaluate rule every N days", "Days" }
                });

            migrationBuilder.InsertData(
                table: "PriceTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Specific price point", "Flat" },
                    { 2, "Percent offset from market price at rule activation", "Percent From Create" },
                    { 3, "Percent offset from market price at trigger execution", "Percent From Execute" }
                });

            migrationBuilder.InsertData(
                table: "SymbolWatchlist",
                columns: new[] { "Id", "IsActive", "Symbol" },
                values: new object[,]
                {
                    { 1, true, "BTC" },
                    { 2, true, "ETH" },
                    { 3, true, "DOGE" },
                    { 4, true, "SHIB" },
                    { 5, true, "SOL" },
                    { 6, true, "XRP" }
                });

            migrationBuilder.InsertData(
                table: "TriggerTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Triggers when price decreases by a percentage", "Down Percent" },
                    { 2, "Triggers when price increases by a percentage", "Up Percent" },
                    { 3, "Triggers when price decreases by a flat amount", "Down Flat" },
                    { 4, "Triggers when price increases by a flat amount", "Up Flat" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidAskHistory_Symbol",
                table: "BidAskHistory",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_BidAskHistory_Symbol_Timestamp",
                table: "BidAskHistory",
                columns: new[] { "Symbol", "Timestamp" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_BidAskHistory_Timestamp",
                table: "BidAskHistory",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_RuleActions_ActionTemplateId",
                table: "RuleActions",
                column: "ActionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAmounts_AmountTemplateId",
                table: "RuleAmounts",
                column: "AmountTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RulePeriodicities_PeriodicityTemplateId",
                table: "RulePeriodicities",
                column: "PeriodicityTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RulePrices_PriceTemplateId",
                table: "RulePrices",
                column: "PriceTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_ActionId",
                table: "Rules",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_AmountId",
                table: "Rules",
                column: "AmountId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PeriodicityId",
                table: "Rules",
                column: "PeriodicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PriceId",
                table: "Rules",
                column: "PriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_RuleSetId",
                table: "Rules",
                column: "RuleSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_TriggerId",
                table: "Rules",
                column: "TriggerId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleTriggers_TriggerTemplateId",
                table: "RuleTriggers",
                column: "TriggerTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolWatchlist_IsActive",
                table: "SymbolWatchlist",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_SymbolWatchlist_Symbol_Currency",
                table: "SymbolWatchlist",
                columns: new[] { "Symbol", "Currency" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidAskHistory");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "SymbolWatchlist");

            migrationBuilder.DropTable(
                name: "RuleActions");

            migrationBuilder.DropTable(
                name: "RuleAmounts");

            migrationBuilder.DropTable(
                name: "RulePeriodicities");

            migrationBuilder.DropTable(
                name: "RulePrices");

            migrationBuilder.DropTable(
                name: "RuleSets");

            migrationBuilder.DropTable(
                name: "RuleTriggers");

            migrationBuilder.DropTable(
                name: "ActionTemplates");

            migrationBuilder.DropTable(
                name: "AmountTemplates");

            migrationBuilder.DropTable(
                name: "PeriodicityTemplates");

            migrationBuilder.DropTable(
                name: "PriceTemplates");

            migrationBuilder.DropTable(
                name: "TriggerTemplates");
        }
    }
}
