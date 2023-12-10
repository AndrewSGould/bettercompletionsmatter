using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBcmRgscTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BcmRgsc_BcmPlayers_BcmPlayerId",
                table: "BcmRgsc");

            migrationBuilder.AlterColumn<long>(
                name: "BcmPlayerId",
                table: "BcmRgsc",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BcmRgsc_BcmPlayers_BcmPlayerId",
                table: "BcmRgsc",
                column: "BcmPlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BcmRgsc_BcmPlayers_BcmPlayerId",
                table: "BcmRgsc");

            migrationBuilder.AlterColumn<long>(
                name: "BcmPlayerId",
                table: "BcmRgsc",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_BcmRgsc_BcmPlayers_BcmPlayerId",
                table: "BcmRgsc",
                column: "BcmPlayerId",
                principalTable: "BcmPlayers",
                principalColumn: "Id");
        }
    }
}
