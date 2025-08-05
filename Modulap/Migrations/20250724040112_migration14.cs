using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class migration14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChickenCoopSize",
                table: "Estimates");

            migrationBuilder.RenameColumn(
                name: "functionalities",
                table: "Estimates",
                newName: "Waterlevelgauge");

            migrationBuilder.RenameColumn(
                name: "EnergyType",
                table: "Estimates",
                newName: "NightMotionSensor");

            migrationBuilder.AddColumn<string>(
                name: "Airqualitymonitoring",
                table: "Estimates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Automaticfeeddispenser",
                table: "Estimates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EnvironmentalMonitoring",
                table: "Estimates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Naturallightingmonitoring",
                table: "Estimates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Airqualitymonitoring",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "Automaticfeeddispenser",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "EnvironmentalMonitoring",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "Naturallightingmonitoring",
                table: "Estimates");

            migrationBuilder.RenameColumn(
                name: "Waterlevelgauge",
                table: "Estimates",
                newName: "functionalities");

            migrationBuilder.RenameColumn(
                name: "NightMotionSensor",
                table: "Estimates",
                newName: "EnergyType");

            migrationBuilder.AddColumn<decimal>(
                name: "ChickenCoopSize",
                table: "Estimates",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
