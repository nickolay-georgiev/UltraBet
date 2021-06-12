using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UltraBet.Data.Migrations
{
    public partial class RenameBetTableToMarket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odds_Bets_BetId",
                table: "Odds");

            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "BetNames");

            migrationBuilder.RenameColumn(
                name: "BetId",
                table: "Odds",
                newName: "MarketId");

            migrationBuilder.RenameIndex(
                name: "IX_Odds_BetId",
                table: "Odds",
                newName: "IX_Odds_MarketId");

            migrationBuilder.CreateTable(
                name: "MarketNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Markets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLive = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MarketNameId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Markets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Markets_MarketNames_MarketNameId",
                        column: x => x.MarketNameId,
                        principalTable: "MarketNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Markets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Markets_IsDeleted",
                table: "Markets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_MarketNameId",
                table: "Markets",
                column: "MarketNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Markets_MatchId",
                table: "Markets",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odds_Markets_MarketId",
                table: "Odds",
                column: "MarketId",
                principalTable: "Markets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Odds_Markets_MarketId",
                table: "Odds");

            migrationBuilder.DropTable(
                name: "Markets");

            migrationBuilder.DropTable(
                name: "MarketNames");

            migrationBuilder.RenameColumn(
                name: "MarketId",
                table: "Odds",
                newName: "BetId");

            migrationBuilder.RenameIndex(
                name: "IX_Odds_MarketId",
                table: "Odds",
                newName: "IX_Odds_BetId");

            migrationBuilder.CreateTable(
                name: "BetNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BetNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BetNameId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsLive = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_BetNames_BetNameId",
                        column: x => x.BetNameId,
                        principalTable: "BetNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_BetNameId",
                table: "Bets",
                column: "BetNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_IsDeleted",
                table: "Bets",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_MatchId",
                table: "Bets",
                column: "MatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Odds_Bets_BetId",
                table: "Odds",
                column: "BetId",
                principalTable: "Bets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
