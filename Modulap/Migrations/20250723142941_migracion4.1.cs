using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modulap.Migrations
{
    /// <inheritdoc />
    public partial class migracion41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Average",
                table: "ComponentLots",
                newName: "AverageLot");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AverageLot",
                table: "ComponentLots",
                newName: "Average");
        }
    }
}
