using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class migracion16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EstimateId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_EstimateId",
                table: "Sales",
                column: "EstimateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Estimates_EstimateId",
                table: "Sales",
                column: "EstimateId",
                principalTable: "Estimates",
                principalColumn: "IdEstimate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Estimates_EstimateId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_EstimateId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "EstimateId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Sales");
        }
    }
}
