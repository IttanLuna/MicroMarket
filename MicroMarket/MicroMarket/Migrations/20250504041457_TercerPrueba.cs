using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroMarket.Migrations
{
    /// <inheritdoc />
    public partial class TercerPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelevisorId",
                table: "Ventas");

            migrationBuilder.AddColumn<int>(
                name: "VentaId1",
                table: "Ventas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_VentaId1",
                table: "Ventas",
                column: "VentaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Ventas_Ventas_VentaId1",
                table: "Ventas",
                column: "VentaId1",
                principalTable: "Ventas",
                principalColumn: "VentaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ventas_Ventas_VentaId1",
                table: "Ventas");

            migrationBuilder.DropIndex(
                name: "IX_Ventas_VentaId1",
                table: "Ventas");

            migrationBuilder.DropColumn(
                name: "VentaId1",
                table: "Ventas");

            migrationBuilder.AddColumn<int>(
                name: "TelevisorId",
                table: "Ventas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
