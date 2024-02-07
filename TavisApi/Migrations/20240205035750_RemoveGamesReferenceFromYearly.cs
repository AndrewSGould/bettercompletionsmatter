using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGamesReferenceFromYearly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_YearlyChallenges_YearlyChallengeId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_YearlyChallengeId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "YearlyChallengeId",
                table: "Games");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "YearlyChallengeId",
                table: "Games",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_YearlyChallengeId",
                table: "Games",
                column: "YearlyChallengeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_YearlyChallenges_YearlyChallengeId",
                table: "Games",
                column: "YearlyChallengeId",
                principalTable: "YearlyChallenges",
                principalColumn: "Id");
        }
    }
}
