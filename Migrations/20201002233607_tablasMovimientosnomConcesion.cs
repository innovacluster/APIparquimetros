using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablasMovimientosnomConcesion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            
            migrationBuilder.AddColumn<double>(
                name: "flt_saldo_anterior",
                table: "tbmovimientos",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "str_nombre_concesion",
                table: "tbmovimientos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_saldo_anterior",
                table: "tbmovimientos");

            migrationBuilder.DropColumn(
                name: "str_nombre_concesion",
                table: "tbmovimientos");

           
        }
    }
}
