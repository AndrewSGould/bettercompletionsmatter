using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class YearsParticipated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstYear",
                table: "BcmMiscStats");

            migrationBuilder.AddColumn<List<DateTime>>(
                name: "YearsParticipated",
                table: "BcmMiscStats",
                type: "timestamp with time zone[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsParticipated",
                table: "BcmMiscStats");

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstYear",
                table: "BcmMiscStats",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
