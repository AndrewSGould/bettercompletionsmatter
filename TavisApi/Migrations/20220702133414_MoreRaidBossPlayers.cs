using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class MoreRaidBossPlayers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 105 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "LastSync", "Name", "Region", "TrueAchievementId" },
                values: new object[] { 175, null, true, null, "GT3OptionFan", null, 257340 });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 175 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 105 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 175 });

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 175);
        }
    }
}
