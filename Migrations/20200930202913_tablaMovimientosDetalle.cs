using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablaMovimientosDetalle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "flt_monto_porcentaje",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_monto_porcentaje",
                table: "tbdetallemovimientos",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_monto_porcentaje",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "flt_monto_porcentaje",
                table: "tbdetallemovimientos");
        }
    }
}
