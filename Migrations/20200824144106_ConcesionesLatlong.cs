using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class ConcesionesLatlong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "str_latitud",
                table: "tbconcesiones",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "str_longitud",
                table: "tbconcesiones",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "str_latitud",
                table: "tbconcesiones");

            migrationBuilder.DropColumn(
                name: "str_longitud",
                table: "tbconcesiones");
        }
    }
}
