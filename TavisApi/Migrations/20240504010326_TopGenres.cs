using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class TopGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerTopGenres",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    GenreId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId1 = table.Column<long>(type: "bigint", nullable: true),
                    GameId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTopGenres", x => new { x.PlayerId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_PlayerTopGenres_BcmPlayers_PlayerId1",
                        column: x => x.PlayerId1,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTopGenres_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerTopGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTopGenres_GameId",
                table: "PlayerTopGenres",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTopGenres_GenreId",
                table: "PlayerTopGenres",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTopGenres_PlayerId1",
                table: "PlayerTopGenres",
                column: "PlayerId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTopGenres");
        }
    }
}
