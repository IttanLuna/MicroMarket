using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMarket.Migrations
{
    /// <inheritdoc />
    public partial class RolVendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rol",
                table: "Vendedores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Vendedores");
        }
    }
}
