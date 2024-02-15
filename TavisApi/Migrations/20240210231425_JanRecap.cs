using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class JanRecap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JanRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    Bucket1Points = table.Column<double>(type: "double precision", nullable: true),
                    Bucket1Comps = table.Column<int>(type: "integer", nullable: true),
                    Bucket2Points = table.Column<double>(type: "double precision", nullable: true),
                    Bucket2Comps = table.Column<int>(type: "integer", nullable: true),
                    Bucket3Points = table.Column<double>(type: "double precision", nullable: true),
                    Bucket3Comps = table.Column<int>(type: "integer", nullable: true),
                    Bucket4Points = table.Column<double>(type: "double precision", nullable: true),
                    Bucket4Comps = table.Column<int>(type: "integer", nullable: true),
                    AllBuckets = table.Column<bool>(type: "boolean", nullable: false),
                    CommunityBonus = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JanRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JanRecap_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JanRecap_PlayerId",
                table: "JanRecap",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JanRecap");
        }
    }
}
