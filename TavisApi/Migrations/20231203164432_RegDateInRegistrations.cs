using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class RegDateInRegistrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Registration",
                table: "BcmPlayers");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "UserRegistration",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "UserRegistration");

            migrationBuilder.AddColumn<DateTime>(
                name: "Registration",
                table: "BcmPlayers",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
