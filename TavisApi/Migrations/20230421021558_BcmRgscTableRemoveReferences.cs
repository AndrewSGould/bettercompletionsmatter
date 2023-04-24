using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class BcmRgscTableRemoveReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BcmRgsc_Games_GameId",
                table: "BcmRgsc");

            migrationBuilder.DropIndex(
                name: "IX_BcmRgsc_GameId",
                table: "BcmRgsc");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RefreshTokenExpiryTime",
                value: new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.CreateIndex(
                name: "IX_BcmRgsc_GameId",
                table: "BcmRgsc",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_BcmRgsc_Games_GameId",
                table: "BcmRgsc",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }
    }
}
