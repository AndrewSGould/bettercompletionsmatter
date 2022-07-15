using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class MoreRaidBossPlayers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 9 });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 120 });

            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 126 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 9 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 120 });

            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 126 });
        }
    }
}
