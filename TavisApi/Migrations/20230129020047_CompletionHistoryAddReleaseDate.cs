using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class CompletionHistoryAddReleaseDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "BcmCompletionHistory",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "BcmCompletionHistory");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
