using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RHWebFront.Migrations
{
    /// <inheritdoc />
    public partial class CreateRuleTables : Migration
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
                name: "PrecisionTemplates",
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
                    table.PrimaryKey("PK_PrecisionTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleOrderPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleOrderPositions", x => x.Id);
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
                name: "RulePrecisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PrecisionTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulePrecisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulePrecisions_PrecisionTemplates_PrecisionTemplateId",
                        column: x => x.PrecisionTemplateId,
                        principalTable: "PrecisionTemplates",
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
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TriggerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PrecisionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AmountId = table.Column<int>(type: "INTEGER", nullable: false),
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
                        name: "FK_Rules_RuleOrderPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "RuleOrderPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rules_RulePrecisions_PrecisionId",
                        column: x => x.PrecisionId,
                        principalTable: "RulePrecisions",
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
                    { 1, "Limit sell order at absolute price", "LimitSellAbsolute" },
                    { 2, "Limit sell order at price relative to rule creation", "LimitSellRelativeAtCreate" },
                    { 3, "Limit sell order at price relative to trigger execution", "LimitSellRelativeAtExecute" },
                    { 4, "Limit buy order at absolute price", "LimitBuyAbsolute" },
                    { 5, "Limit buy order at price relative to rule creation", "LimitBuyRelativeAtCreate" },
                    { 6, "Limit buy order at price relative to trigger execution", "LimitBuyRelativeAtExecute" },
                    { 7, "Market sell order executed immediately at current market price", "MarketSell" },
                    { 8, "Market buy order executed immediately at current market price", "MarketBuy" }
                });

            migrationBuilder.InsertData(
                table: "AmountTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Specific quantity of the asset", "Absolute" },
                    { 2, "Percentage of available holdings", "Percent" },
                    { 3, "All available holdings", "All" }
                });

            migrationBuilder.InsertData(
                table: "PrecisionTemplates",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Evaluate rule every N seconds", "Seconds" },
                    { 2, "Evaluate rule every N minutes", "Minutes" },
                    { 3, "Evaluate rule every N hours", "Hours" },
                    { 4, "Evaluate rule every N days", "Days" }
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
                    { 1, "Triggers when price decreases by a percentage", "DownPercent" },
                    { 2, "Triggers when price increases by a percentage", "UpPercent" },
                    { 3, "Triggers when price decreases by an absolute amount", "DownAbsolute" },
                    { 4, "Triggers when price increases by an absolute amount", "UpAbsolute" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuleActions_ActionTemplateId",
                table: "RuleActions",
                column: "ActionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleAmounts_AmountTemplateId",
                table: "RuleAmounts",
                column: "AmountTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RulePrecisions_PrecisionTemplateId",
                table: "RulePrecisions",
                column: "PrecisionTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_ActionId",
                table: "Rules",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_AmountId",
                table: "Rules",
                column: "AmountId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PositionId",
                table: "Rules",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PrecisionId",
                table: "Rules",
                column: "PrecisionId");

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

            // Add triggers to update UpdatedAt on UPDATE for all rule tables
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetActionTemplateUpdatedAt
                AFTER UPDATE ON ActionTemplates
                FOR EACH ROW
                BEGIN
                    UPDATE ActionTemplates SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetAmountTemplateUpdatedAt
                AFTER UPDATE ON AmountTemplates
                FOR EACH ROW
                BEGIN
                    UPDATE AmountTemplates SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetPrecisionTemplateUpdatedAt
                AFTER UPDATE ON PrecisionTemplates
                FOR EACH ROW
                BEGIN
                    UPDATE PrecisionTemplates SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleOrderPositionUpdatedAt
                AFTER UPDATE ON RuleOrderPositions
                FOR EACH ROW
                BEGIN
                    UPDATE RuleOrderPositions SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleSetUpdatedAt
                AFTER UPDATE ON RuleSets
                FOR EACH ROW
                BEGIN
                    UPDATE RuleSets SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetTriggerTemplateUpdatedAt
                AFTER UPDATE ON TriggerTemplates
                FOR EACH ROW
                BEGIN
                    UPDATE TriggerTemplates SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleActionUpdatedAt
                AFTER UPDATE ON RuleActions
                FOR EACH ROW
                BEGIN
                    UPDATE RuleActions SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleAmountUpdatedAt
                AFTER UPDATE ON RuleAmounts
                FOR EACH ROW
                BEGIN
                    UPDATE RuleAmounts SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRulePrecisionUpdatedAt
                AFTER UPDATE ON RulePrecisions
                FOR EACH ROW
                BEGIN
                    UPDATE RulePrecisions SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleTriggerUpdatedAt
                AFTER UPDATE ON RuleTriggers
                FOR EACH ROW
                BEGIN
                    UPDATE RuleTriggers SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE TRIGGER SetRuleUpdatedAt
                AFTER UPDATE ON Rules
                FOR EACH ROW
                BEGIN
                    UPDATE Rules SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop triggers for all rule tables
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetActionTemplateUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetAmountTemplateUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetPrecisionTemplateUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleOrderPositionUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleSetUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetTriggerTemplateUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleActionUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleAmountUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRulePrecisionUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleTriggerUpdatedAt;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS SetRuleUpdatedAt;");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "RuleActions");

            migrationBuilder.DropTable(
                name: "RuleAmounts");

            migrationBuilder.DropTable(
                name: "RuleOrderPositions");

            migrationBuilder.DropTable(
                name: "RulePrecisions");

            migrationBuilder.DropTable(
                name: "RuleSets");

            migrationBuilder.DropTable(
                name: "RuleTriggers");

            migrationBuilder.DropTable(
                name: "ActionTemplates");

            migrationBuilder.DropTable(
                name: "AmountTemplates");

            migrationBuilder.DropTable(
                name: "PrecisionTemplates");

            migrationBuilder.DropTable(
                name: "TriggerTemplates");

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SymbolWatchlist",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
