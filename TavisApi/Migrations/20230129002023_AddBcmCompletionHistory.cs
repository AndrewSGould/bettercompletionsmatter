using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class AddBcmCompletionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BcmCompletionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GameId = table.Column<int>(type: "integer", nullable: true),
                    SiteRatio = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmCompletionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BcmCompletionHistory_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_BcmCompletionHistory_GameId",
                table: "BcmCompletionHistory",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BcmCompletionHistory");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
