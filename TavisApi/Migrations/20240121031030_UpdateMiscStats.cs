using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMiscStats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.DropColumn(
                name: "LifetimeRgscCompletions",
                table: "BcmMiscStats");

          migrationBuilder.DropColumn(
                name: "FullCombos",
                table: "BcmMiscStats");

          migrationBuilder.DropColumn(
                name: "YearsParticipated",
                table: "BcmMiscStats");

          migrationBuilder.AddColumn<string>(
                name: "HistoricalStats",
                table: "BcmMiscStats",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.AddColumn<int>(
                name: "LifetimeRgscCompletions",
                table: "BcmMiscStats",
                type: "integer",
                nullable: true);

          migrationBuilder.AddColumn<List<DateTime>>(
                name: "FullCombos",
                table: "BcmMiscStats",
                type: "timestamp with time zone[]",
                nullable: true);

          migrationBuilder.AddColumn<List<DateTime>>(
                name: "YearsParticipated",
                table: "BcmMiscStats",
                type: "timestamp with time zone[]",
                nullable: true);

          migrationBuilder.DropColumn(
                name: "HistoricalStats",
                table: "BcmMiscStats");
        }
    }
}
