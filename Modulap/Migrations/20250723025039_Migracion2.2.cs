using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class Migracion22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentCostings_ComponentLosses_ComponentLossId",
                table: "ComponentCostings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentProductionId",
                table: "ComponentCostings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentLossId",
                table: "ComponentCostings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentCostings_ComponentLosses_ComponentLossId",
                table: "ComponentCostings",
                column: "ComponentLossId",
                principalTable: "ComponentLosses",
                principalColumn: "IdComponentLoss");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentCostings_ComponentLosses_ComponentLossId",
                table: "ComponentCostings");

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentProductionId",
                table: "ComponentCostings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ComponentLossId",
                table: "ComponentCostings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentCostings_ComponentLosses_ComponentLossId",
                table: "ComponentCostings",
                column: "ComponentLossId",
                principalTable: "ComponentLosses",
                principalColumn: "IdComponentLoss",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
