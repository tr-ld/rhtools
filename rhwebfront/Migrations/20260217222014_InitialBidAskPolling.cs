using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RHWebFront.Migrations
{
    /// <inheritdoc />
    public partial class InitialBidAskPolling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "SymbolWatchlist");
        }
    }
}
