using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class September : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeptemberRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    StreakCount = table.Column<int>(type: "integer", nullable: true),
                    StreakedGames = table.Column<string>(type: "text", nullable: true),
                    CommunityBonus = table.Column<int>(type: "integer", nullable: false),
                    Participation = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeptemberRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeptemberRecap_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeptemberRecap_PlayerId",
                table: "SeptemberRecap",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeptemberRecap");
        }
    }
}
