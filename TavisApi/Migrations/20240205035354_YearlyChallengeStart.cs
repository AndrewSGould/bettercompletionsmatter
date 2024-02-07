using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class YearlyChallengeStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "YearlyChallengeId",
                table: "Games",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "YearlyChallenges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyChallenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerYearlyChallenges",
                columns: table => new
                {
                    YearlyChallengeId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: false),
                    YearlyChallengeId1 = table.Column<long>(type: "bigint", nullable: false),
                    WriteIn = table.Column<string>(type: "text", nullable: true),
                    Reasoning = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerYearlyChallenges", x => new { x.GameId, x.YearlyChallengeId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerYearlyChallenges_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerYearlyChallenges_YearlyChallenges_YearlyChallengeId1",
                        column: x => x.YearlyChallengeId1,
                        principalTable: "YearlyChallenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "YearlyChallenges",
                columns: new[] { "Id", "Category", "Description", "Title" },
                values: new object[] { 1L, 0, "Complete any game with an audible fictional language", "A Wise Wookie Once Said" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_YearlyChallengeId",
                table: "Games",
                column: "YearlyChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerYearlyChallenges_YearlyChallengeId1",
                table: "PlayerYearlyChallenges",
                column: "YearlyChallengeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_YearlyChallenges_YearlyChallengeId",
                table: "Games",
                column: "YearlyChallengeId",
                principalTable: "YearlyChallenges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_YearlyChallenges_YearlyChallengeId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "PlayerYearlyChallenges");

            migrationBuilder.DropTable(
                name: "YearlyChallenges");

            migrationBuilder.DropIndex(
                name: "IX_Games_YearlyChallengeId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "YearlyChallengeId",
                table: "Games");
        }
    }
}
