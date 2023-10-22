using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Logins");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Logins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Xuid = table.Column<string>(type: "text", nullable: true),
                    DiscordId = table.Column<int>(type: "integer", nullable: true),
                    Gamertag = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
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
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logins_Users_UserId",
                table: "Logins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Users_UserId",
                table: "Logins");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Logins_UserId",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Logins");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Logins",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Username" },
                values: new object[] { 1L, "def@123", null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "johndoe" });

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "RoleId",
                value: new Guid("d831bf93-433d-4af7-ae77-4106dae1d651"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "RoleId",
                value: new Guid("20d2cb34-0f77-4861-9a58-5df4681ba2ca"));

            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "RoleId",
                value: new Guid("20fcfb8c-346f-4cd7-a68e-dc2fedf2def3"));
        }
    }
}
