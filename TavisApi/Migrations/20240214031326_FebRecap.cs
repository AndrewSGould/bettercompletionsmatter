using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FebRecap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FebRecap",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    Gamertag = table.Column<string>(type: "text", nullable: false),
                    BiCompletion = table.Column<int>(type: "integer", nullable: false),
                    TriCompletion = table.Column<int>(type: "integer", nullable: false),
                    QuadCompletion = table.Column<int>(type: "integer", nullable: false),
                    QuintCompletion = table.Column<int>(type: "integer", nullable: false),
                    SexCompletion = table.Column<int>(type: "integer", nullable: false),
                    SepCompletion = table.Column<int>(type: "integer", nullable: false),
                    OctCompletion = table.Column<int>(type: "integer", nullable: false),
                    DecCompletion = table.Column<int>(type: "integer", nullable: false),
                    UndeCompletion = table.Column<int>(type: "integer", nullable: false),
                    CommunityBonus = table.Column<bool>(type: "boolean", nullable: false),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: false),
                    PlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FebRecap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FebRecap_BcmPlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "BcmPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FebRecap_PlayerId",
                table: "FebRecap",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FebRecap");
        }
    }
}
