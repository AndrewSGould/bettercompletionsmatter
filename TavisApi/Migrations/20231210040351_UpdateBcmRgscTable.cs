using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBcmRgscTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RerollDate",
                table: "BcmRgsc",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RerollDate",
                table: "BcmRgsc");
        }
    }
}
