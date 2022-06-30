using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    public partial class NewPlayerForRaidEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Area", "IsActive", "LastSync", "Name", "Region", "TrueAchievementId" },
                values: new object[,]
                {
                    { 157, null, true, null, "xI The Rock Ix", null, 408827 },
                    { 158, null, true, null, "tatersoup19", null, 85256 },
                    { 159, null, true, null, "nightw0lf", null, 347191 },
                    { 160, null, true, null, "N龱T廾 T廾A G龱D", null, 1725 },
                    { 161, null, true, null, "emz fergi", null, 702307 },
                    { 162, null, true, null, "Inigomontoya80", null, 262143 },
                    { 163, null, true, null, "PRTM CLUESCROL", null, 597081 },
                    { 164, null, true, null, "AZ Mongoose", null, 48289 },
                    { 165, null, true, null, "VulgarLatin", null, 76517 },
                    { 166, null, true, null, "TobyLinn", null, 643897 },
                    { 167, null, true, null, "Jblacq", null, 52223 },
                    { 168, null, true, null, "Enigma Gamer 77", null, 370170 },
                    { 169, null, true, null, "Xpovos", null, 318602 },
                    { 170, null, true, null, "Shadow", null, 409281 },
                    { 171, null, true, null, "Ahayzo", null, 276943 },
                    { 172, null, true, null, "Darklord Davis", null, 364130 },
                    { 173, null, true, null, "logicslayer", null, 97393 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 173);
        }
    }
}
