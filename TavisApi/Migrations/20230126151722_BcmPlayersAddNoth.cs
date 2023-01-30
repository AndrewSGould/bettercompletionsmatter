using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class BcmPlayersAddNoth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 1, 160 });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { true, "N0TH THA G0D" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 1, 160 });

            migrationBuilder.InsertData(
                table: "Contests",
                columns: new[] { "Id", "EndDate", "Name", "StartDate" },
                values: new object[] { 2, new DateTime(2022, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Raid Boss", new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 160,
                columns: new[] { "IsActive", "Name" },
                values: new object[] { false, "N龱T廾 T廾A G龱D" });
        }
    }
}
