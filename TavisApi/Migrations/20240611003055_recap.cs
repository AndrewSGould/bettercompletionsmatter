using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class recap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Games",
                table: "MayRecap");

            migrationBuilder.AddColumn<long>(
                name: "MayRecapId",
                table: "BcmPlayerGames",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BcmPlayerGames_MayRecapId",
                table: "BcmPlayerGames",
                column: "MayRecapId");

            migrationBuilder.AddForeignKey(
                name: "FK_BcmPlayerGames_MayRecap_MayRecapId",
                table: "BcmPlayerGames",
                column: "MayRecapId",
                principalTable: "MayRecap",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BcmPlayerGames_MayRecap_MayRecapId",
                table: "BcmPlayerGames");

            migrationBuilder.DropIndex(
                name: "IX_BcmPlayerGames_MayRecapId",
                table: "BcmPlayerGames");

            migrationBuilder.DropColumn(
                name: "MayRecapId",
                table: "BcmPlayerGames");

            migrationBuilder.AddColumn<int>(
                name: "Games",
                table: "MayRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
