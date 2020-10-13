using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablaMovimientos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "flt_porcentaje_comision",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_total_con_comision",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_porcentaje_comision",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "flt_total_con_comision",
                table: "tbmovimientos");
        }
    }
}
