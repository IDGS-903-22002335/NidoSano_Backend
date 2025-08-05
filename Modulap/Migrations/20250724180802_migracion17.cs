using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class migracion17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_ChickenCoops_ChickenCoopId",
                table: "SaleDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChickenCoopId",
                table: "SaleDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_ChickenCoops_ChickenCoopId",
                table: "SaleDetails",
                column: "ChickenCoopId",
                principalTable: "ChickenCoops",
                principalColumn: "IdChickenCoop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleDetails_ChickenCoops_ChickenCoopId",
                table: "SaleDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "ChickenCoopId",
                table: "SaleDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleDetails_ChickenCoops_ChickenCoopId",
                table: "SaleDetails",
                column: "ChickenCoopId",
                principalTable: "ChickenCoops",
                principalColumn: "IdChickenCoop",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
