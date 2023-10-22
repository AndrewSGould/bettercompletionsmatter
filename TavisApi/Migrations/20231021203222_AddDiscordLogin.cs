using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscordLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginUserRole");

            migrationBuilder.DropColumn(
                name: "DiscordId",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "UserRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserRoleId",
                table: "Logins",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscordLogins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscordId = table.Column<int>(type: "integer", nullable: false),
                    TokenType = table.Column<string>(type: "text", nullable: true),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("cbeaba8f-e778-44b5-8e52-6c0a20008bed"), null });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("4f3617bb-654d-4b39-873f-ed1d5d404019"), null });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("eb8ea9db-2b4e-4909-b394-0450d7ae5dd2"), null });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserRoleId",
                table: "Logins",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordLogins_UserId",
                table: "DiscordLogins",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_UserRoles_UserRoleId",
                table: "Logins",
                column: "UserRoleId",
                principalTable: "UserRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_UserRoles_UserRoleId",
                table: "Logins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropTable(
                name: "DiscordLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Logins_UserRoleId",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "UserRoleId",
                table: "Logins");

            migrationBuilder.AddColumn<int>(
                name: "DiscordId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoginUserRole",
                columns: table => new
                {
                    LoginsId = table.Column<long>(type: "bigint", nullable: false),
                    UserRolesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginUserRole", x => new { x.LoginsId, x.UserRolesId });
                    table.ForeignKey(
                        name: "FK_LoginUserRole_Logins_LoginsId",
                        column: x => x.LoginsId,
                        principalTable: "Logins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoginUserRole_UserRoles_UserRolesId",
                        column: x => x.UserRolesId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleId",
                value: new Guid("0e85b01d-cea8-4996-8ecb-53a087bf90a5"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleId",
                value: new Guid("01b8e103-ab0b-4ada-b1d4-3684814b3f0f"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleId",
                value: new Guid("6f6f2918-c9c5-48c2-81e3-51d774f5036e"));

            migrationBuilder.CreateIndex(
                name: "IX_LoginUserRole_UserRolesId",
                table: "LoginUserRole",
                column: "UserRolesId");
        }
    }
}
