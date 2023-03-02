using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class AddBcmStatsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BcmStats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rank = table.Column<int>(type: "integer", nullable: true),
                    RankMovement = table.Column<int>(type: "integer", nullable: true),
                    Completions = table.Column<int>(type: "integer", nullable: true),
                    AverageRatio = table.Column<double>(type: "double precision", nullable: true),
                    HighestRatio = table.Column<double>(type: "double precision", nullable: true),
                    AverageTimeEstimate = table.Column<double>(type: "double precision", nullable: true),
                    HighestTimeEstimate = table.Column<double>(type: "double precision", nullable: true),
                    AveragePoints = table.Column<double>(type: "double precision", nullable: true),
                    BasePoints = table.Column<double>(type: "double precision", nullable: true),
                    BonusPoints = table.Column<double>(type: "double precision", nullable: true),
                    TotalPoints = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BcmStats", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BcmStats");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
