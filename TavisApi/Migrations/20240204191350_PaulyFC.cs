using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class PaulyFC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 79L,
                column: "HistoricalStats",
                value: "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":20},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":true,\"Placement\":11},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":49}]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 79L,
                column: "HistoricalStats",
                value: "[{\"Year\":2021,\"Rgsc\":1,\"FullCombo\":false,\"Placement\":20},{\"Year\":2022,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":11},{\"Year\":2023,\"Rgsc\":12,\"FullCombo\":false,\"Placement\":49}]");
        }
    }
}
