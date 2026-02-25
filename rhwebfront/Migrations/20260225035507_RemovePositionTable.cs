using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RHWebFront.Migrations
{
    /// <inheritdoc />
    public partial class RemovePositionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rules_RuleOrderPositions_PositionId",
                table: "Rules");

            migrationBuilder.DropTable(
                name: "RuleOrderPositions");

            migrationBuilder.DropIndex(
                name: "IX_Rules_PositionId",
                table: "Rules");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "Rules",
                newName: "Position");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "RuleActions",
                type: "decimal(38,18)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "RuleActions");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Rules",
                newName: "PositionId");

            migrationBuilder.CreateTable(
                name: "RuleOrderPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')"),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "DATETIME('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleOrderPositions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rules_PositionId",
                table: "Rules",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_RuleOrderPositions_PositionId",
                table: "Rules",
                column: "PositionId",
                principalTable: "RuleOrderPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
