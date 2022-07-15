using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class MoreRaidBossPlayers3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[,]
                {
                    { 2, 72 },
                    { 2, 93 },
                    { 2, 124 }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "LastSync", "Name", "Region", "TrueAchievementId" },
                values: new object[] { 176, null, true, null, "kungfuskills", null, 357761 });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 176 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 72 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 93 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 124 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 176 });

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 176);
        }
    }
}
