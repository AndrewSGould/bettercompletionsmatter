using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class DiscordIdIntToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DiscordId",
                table: "DiscordLogins",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleId",
                value: new Guid("9bcaa509-1096-4b80-9177-2e723f08a674"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleId",
                value: new Guid("741f8f18-c98d-4908-b5b9-2a0b1ba7643d"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleId",
                value: new Guid("759d0d53-32b1-4313-9467-2c5614c62ba0"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiscordId",
                table: "DiscordLogins",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleId",
                value: new Guid("cbeaba8f-e778-44b5-8e52-6c0a20008bed"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleId",
                value: new Guid("4f3617bb-654d-4b39-873f-ed1d5d404019"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleId",
                value: new Guid("eb8ea9db-2b4e-4909-b394-0450d7ae5dd2"));
        }
    }
}
