using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FixScarData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 75L,
                column: "HistoricalStats",
                value: "[{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":7}]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 75L,
                column: "HistoricalStats",
                value: "[{\"Year\":2023,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":7}]");
        }
    }
}
