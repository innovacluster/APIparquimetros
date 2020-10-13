using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tarifasupdtae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "bool_cobro_fraccio",
                table: "tbtarifas",
                newName: "bool_cobro_fraccion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "bool_cobro_fraccion",
                table: "tbtarifas",
                newName: "bool_cobro_fraccio");
        }
    }
}
