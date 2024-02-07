using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixExtraIdInPlayerYearly4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<long>(
                name: "YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId1",
                principalTable: "YearlyChallenges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges");

            migrationBuilder.AlterColumn<long>(
                name: "YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId1",
                principalTable: "YearlyChallenges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
