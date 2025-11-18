using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballPoolApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialFootballPool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Week = table.Column<int>(type: "INTEGER", nullable: false),
                    League = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Favorite = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Underdog = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Spread = table.Column<double>(type: "REAL", nullable: false),
                    GameTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FavoriteScore = table.Column<int>(type: "INTEGER", nullable: true),
                    UnderdogScore = table.Column<int>(type: "INTEGER", nullable: true),
                    IsScored = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Picks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SelectedTeam = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: true),
                    PickedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Picks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Picks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Picks_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Picks_GameId",
                table: "Picks",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Picks_UserId_GameId",
                table: "Picks",
                columns: new[] { "UserId", "GameId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Picks");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
