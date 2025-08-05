using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class Migracion37 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPriceActual",
                table: "ComponentLots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "UnitPriceActual",
                table: "ComponentLots",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
