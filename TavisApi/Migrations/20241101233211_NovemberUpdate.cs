using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class NovemberUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NovemberRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    Podium2019_1st = table.Column<int>(type: "integer", nullable: false),
                    Podium2019_2nd = table.Column<int>(type: "integer", nullable: false),
                    Podium2019_3rd = table.Column<int>(type: "integer", nullable: false),
                    Podium2020_1st = table.Column<int>(type: "integer", nullable: false),
                    Podium2020_2nd = table.Column<int>(type: "integer", nullable: false),
                    Podium2020_3rd = table.Column<int>(type: "integer", nullable: false),
                    Podium2021_1st = table.Column<int>(type: "integer", nullable: false),
                    Podium2021_2nd = table.Column<int>(type: "integer", nullable: false),
                    Podium2021_3rd = table.Column<int>(type: "integer", nullable: false),
                    Podium2022_1st = table.Column<int>(type: "integer", nullable: false),
                    Podium2022_2nd = table.Column<int>(type: "integer", nullable: false),
                    Podium2022_3rd = table.Column<int>(type: "integer", nullable: false),
                    Podium2023_1st = table.Column<int>(type: "integer", nullable: false),
                    Podium2023_2nd = table.Column<int>(type: "integer", nullable: false),
                    Podium2023_3rd = table.Column<int>(type: "integer", nullable: false),
                    CommunityBonus = table.Column<int>(type: "integer", nullable: false),
                    Participation = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NovemberRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NovemberRecap_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NovemberRecap_BcmPlayerId",
                table: "NovemberRecap",
                column: "BcmPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NovemberRecap");
        }
    }
}
