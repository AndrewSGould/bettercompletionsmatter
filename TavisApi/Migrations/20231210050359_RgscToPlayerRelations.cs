using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class RgscToPlayerRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "BcmRgsc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PlayerId",
                table: "BcmRgsc",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
