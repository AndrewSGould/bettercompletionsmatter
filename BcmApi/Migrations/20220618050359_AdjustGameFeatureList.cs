using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BcmApi.Migrations
{
    public partial class AdjustGameFeatureList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TransferableProgress",
                table: "FeatureLists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferableProgress",
                table: "FeatureLists");
        }
    }
}
