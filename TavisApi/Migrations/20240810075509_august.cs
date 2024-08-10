using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class august : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AugustRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    AchievementCount = table.Column<int>(type: "integer", nullable: true),
                    BloodAngelTribute = table.Column<bool>(type: "boolean", nullable: false),
                    ImperialFistTribute = table.Column<bool>(type: "boolean", nullable: false),
                    SpaceWolvesTribute = table.Column<bool>(type: "boolean", nullable: false),
                    UltramarinesTribute = table.Column<bool>(type: "boolean", nullable: false),
                    DeathwatchTribute = table.Column<bool>(type: "boolean", nullable: false),
                    CommunityBonus = table.Column<int>(type: "integer", nullable: false),
                    Participation = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AugustRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AugustRecap_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AugustRecap_PlayerId",
                table: "AugustRecap",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AugustRecap");
        }
    }
}
