using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class JunRecap2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JunRecap_BcmPlayers_BcmPlayerId",
                table: "JunRecap");

            migrationBuilder.DropIndex(
                name: "IX_JunRecap_BcmPlayerId",
                table: "JunRecap");

            migrationBuilder.DropColumn(
                name: "BcmPlayerId",
                table: "JunRecap");

            migrationBuilder.CreateIndex(
                name: "IX_JunRecap_PlayerId",
                table: "JunRecap",
                column: "PlayerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JunRecap_BcmPlayers_PlayerId",
                table: "JunRecap",
                column: "PlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JunRecap_BcmPlayers_PlayerId",
                table: "JunRecap");

            migrationBuilder.DropIndex(
                name: "IX_JunRecap_PlayerId",
                table: "JunRecap");

            migrationBuilder.AddColumn<long>(
                name: "BcmPlayerId",
                table: "JunRecap",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JunRecap_BcmPlayerId",
                table: "JunRecap",
                column: "BcmPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_JunRecap_BcmPlayers_BcmPlayerId",
                table: "JunRecap",
                column: "BcmPlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id");
        }
    }
}
