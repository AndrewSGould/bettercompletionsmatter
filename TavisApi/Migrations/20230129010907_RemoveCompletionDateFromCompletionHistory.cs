using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class RemoveCompletionDateFromCompletionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionDate",
                table: "BcmCompletionHistory");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BcmCompletionHistory",
                type: "text",
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
                name: "Title",
                table: "BcmCompletionHistory");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletionDate",
                table: "BcmCompletionHistory",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
