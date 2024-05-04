using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class TopGenres2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId",
                table: "PlayerTopGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTopGenres_Genres_GenreId",
                table: "PlayerTopGenres");

            migrationBuilder.DropIndex(
                name: "IX_PlayerTopGenres_GenreId",
                table: "PlayerTopGenres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlayerTopGenres_GenreId",
                table: "PlayerTopGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId",
                table: "PlayerTopGenres",
                column: "PlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTopGenres_Genres_GenreId",
                table: "PlayerTopGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
