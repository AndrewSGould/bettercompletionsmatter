using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMagicalChildStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BcmMiscStats",
                columns: new[] { "Id", "HistoricalStats", "PlayerId" },
                values: new object[] { 105L, "[{\"Year\":2023,\"Rgsc\":0,\"FullCombo\":false,\"Placement\":111}]", 166L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BcmMiscStats",
                keyColumn: "Id",
                keyValue: 105L);
        }
    }
}
