using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class RevertCompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_Games_GameId",
                table: "PlayerYearlyChallenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "PlayerYearlyChallenges",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges",
                columns: new[] { "YearlyChallengeId", "PlayerId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_Games_GameId",
                table: "PlayerYearlyChallenges",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_Games_GameId",
                table: "PlayerYearlyChallenges");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "PlayerYearlyChallenges",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges",
                columns: new[] { "YearlyChallengeId", "GameId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_Games_GameId",
                table: "PlayerYearlyChallenges",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
