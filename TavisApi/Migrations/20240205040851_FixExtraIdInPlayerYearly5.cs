using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixExtraIdInPlayerYearly5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges",
                columns: new[] { "YearlyChallengeId", "PlayerId" });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerYearlyChallenges_GameId",
                table: "PlayerYearlyChallenges",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges");

            migrationBuilder.DropIndex(
                name: "IX_PlayerYearlyChallenges_GameId",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerYearlyChallenges",
                table: "PlayerYearlyChallenges",
                columns: new[] { "GameId", "YearlyChallengeId", "PlayerId" });
        }
    }
}
