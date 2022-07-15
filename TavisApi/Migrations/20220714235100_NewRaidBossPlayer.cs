using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class NewRaidBossPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PlayerContests",
                columns: new[] { "ContestId", "PlayerId" },
                values: new object[] { 2, 118 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlayerContests",
                keyColumns: new[] { "ContestId", "PlayerId" },
                keyValues: new object[] { 2, 118 });
        }
    }
}
