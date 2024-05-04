using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixTopGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId1",
                table: "PlayerTopGenres");

            migrationBuilder.DropIndex(
                name: "IX_PlayerTopGenres_PlayerId1",
                table: "PlayerTopGenres");

            migrationBuilder.DropColumn(
                name: "PlayerId1",
                table: "PlayerTopGenres");

            migrationBuilder.AlterColumn<long>(
                name: "PlayerId",
                table: "PlayerTopGenres",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId",
                table: "PlayerTopGenres",
                column: "PlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId",
                table: "PlayerTopGenres");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "PlayerTopGenres",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "PlayerId1",
                table: "PlayerTopGenres",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTopGenres_PlayerId1",
                table: "PlayerTopGenres",
                column: "PlayerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTopGenres_BcmPlayers_PlayerId1",
                table: "PlayerTopGenres",
                column: "PlayerId1",
                principalTable: "BcmPlayers",
                principalColumn: "Id");
        }
    }
}
