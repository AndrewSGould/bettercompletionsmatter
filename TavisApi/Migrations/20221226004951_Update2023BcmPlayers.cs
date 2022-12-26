using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class Update2023BcmPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 44,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 89,
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 140,
                column: "IsActive",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 44,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 89,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 140,
                column: "IsActive",
                value: false);
        }
    }
}
