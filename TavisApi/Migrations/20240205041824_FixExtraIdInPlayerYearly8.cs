using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixExtraIdInPlayerYearly8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges");

            migrationBuilder.DropIndex(
                name: "IX_PlayerYearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges");

            migrationBuilder.DropColumn(
                name: "YearlyChallengeId1",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<long>(
                name: "YearlyChallengeId",
                table: "PlayerYearlyChallenges",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId",
                principalTable: "YearlyChallenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<int>(
                name: "YearlyChallengeId",
                table: "PlayerYearlyChallenges",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerYearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId1",
                principalTable: "YearlyChallenges",
                principalColumn: "Id");
        }
    }
}
