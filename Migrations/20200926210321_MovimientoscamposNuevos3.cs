using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class MovimientoscamposNuevos3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo_vehiculo",
                table: "tbmovimientos",
                nullable: true,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "tipo_vehiculo",
                table: "tbmovimientos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
