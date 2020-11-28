using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class migrationConcesionesCiudades : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "intidciudad_cat",
                table: "tbconcesiones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "str_ciudad",
                table: "tbconcesiones",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intidciudad_cat",
                table: "tbconcesiones");

            migrationBuilder.DropColumn(
                name: "str_ciudad",
                table: "tbconcesiones");
        }
    }
}
