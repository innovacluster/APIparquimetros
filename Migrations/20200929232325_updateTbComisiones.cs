using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class updateTbComisiones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "flt_porcentaje_comision",
                table: "tbsaldo",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_total_con_comision",
                table: "tbsaldo",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_porcentaje_comision",
                table: "tbdetallemovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "flt_total_con_comision",
                table: "tbdetallemovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "str_tipo",
                table: "tbcomisiones",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_porcentaje_comision",
                table: "tbsaldo");

            migrationBuilder.DropColumn(
                name: "flt_total_con_comision",
                table: "tbsaldo");

            migrationBuilder.DropColumn(
                name: "flt_porcentaje_comision",
                table: "tbdetallemovimientos");

            migrationBuilder.DropColumn(
                name: "flt_total_con_comision",
                table: "tbdetallemovimientos");

            migrationBuilder.DropColumn(
                name: "str_tipo",
                table: "tbcomisiones");
        }
    }
}
