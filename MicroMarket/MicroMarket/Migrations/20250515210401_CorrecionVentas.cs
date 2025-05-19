using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMarket.Migrations
{
    /// <inheritdoc />
    public partial class CorrecionVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anio",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "Ventas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Anio",
                table: "Ventas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mes",
                table: "Ventas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
