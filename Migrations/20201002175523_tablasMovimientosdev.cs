using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablasMovimientosdev : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "flt_monto_devolucion",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_monto_porc_devolucion",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_monto_real",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_total_dev_con_comision",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "int_tiempo_comprado",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_tiempo_devuelto",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_monto_devolucion",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "flt_monto_porc_devolucion",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "flt_monto_real",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "flt_total_dev_con_comision",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "int_tiempo_comprado",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "int_tiempo_devuelto",
                table: "tbmovimientos");
        }
    }
}
