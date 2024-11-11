using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class NovemberUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityBonus",
                table: "NovemberRecap");

            migrationBuilder.AddColumn<bool>(
                name: "CommunityBonusQualified",
                table: "NovemberRecap",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityBonusQualified",
                table: "NovemberRecap");

            migrationBuilder.AddColumn<int>(
                name: "CommunityBonus",
                table: "NovemberRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
