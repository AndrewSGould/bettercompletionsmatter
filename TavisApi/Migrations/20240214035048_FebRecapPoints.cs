using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TavisApi.Migrations
{
    /// <inheritdoc />
    public partial class FebRecapPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BiPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DecPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DuodePoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OctPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuadPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuintPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SepPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SexPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TriPoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UndePoints",
                table: "FebRecap",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "DecPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "DuodePoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "OctPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "QuadPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "QuintPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "SepPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "SexPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "TriPoints",
                table: "FebRecap");

            migrationBuilder.DropColumn(
                name: "UndePoints",
                table: "FebRecap");
        }
    }
}
