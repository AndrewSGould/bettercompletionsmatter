using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class MarRecap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BcmMonthlyStats");

            migrationBuilder.CreateTable(
                name: "MarRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    BestBounty = table.Column<string>(type: "text", nullable: false),
                    BountyCount = table.Column<int>(type: "integer", nullable: false),
                    BountiesClaimed = table.Column<string>(type: "text", nullable: false),
                    CommunityBonus = table.Column<bool>(type: "boolean", nullable: false),
                    Participation = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarRecap_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarRecap_BcmPlayerId",
                table: "MarRecap",
                column: "BcmPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarRecap");

            migrationBuilder.CreateTable(
                name: "BcmMonthlyStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: false),
                    AllBuckets = table.Column<bool>(type: "boolean", nullable: true),
                    BonusPoints = table.Column<int>(type: "integer", nullable: false),
                    Challenge = table.Column<int>(type: "integer", nullable: false),
                    CommunityBonus = table.Column<bool>(type: "boolean", nullable: true),
                    Participation = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmMonthlyStats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmMonthlyStats_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BcmMonthlyStats_BcmPlayerId",
                table: "BcmMonthlyStats",
                column: "BcmPlayerId");
        }
    }
}
