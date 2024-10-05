using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class Oct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OctoberRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    BoneCount = table.Column<int>(type: "integer", nullable: true),
                    CrimsonCurseRitual = table.Column<bool>(type: "boolean", nullable: false),
                    DreadRitual = table.Column<bool>(type: "boolean", nullable: false),
                    MarkOfTheBeast1Ritual = table.Column<bool>(type: "boolean", nullable: false),
                    MarkOfTheBeast2Ritual = table.Column<bool>(type: "boolean", nullable: false),
                    MarkOfTheBeast3Ritual = table.Column<bool>(type: "boolean", nullable: false),
                    CommunityBonus = table.Column<int>(type: "integer", nullable: false),
                    Participation = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false),
                    BcmPlayerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OctoberRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OctoberRecap_BcmPlayers_BcmPlayerId",
                        column: x => x.BcmPlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OctoberRecap_BcmPlayerId",
                table: "OctoberRecap",
                column: "BcmPlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OctoberRecap");
        }
    }
}
