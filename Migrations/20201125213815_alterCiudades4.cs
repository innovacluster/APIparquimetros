using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class alterCiudades4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "str_desc_ciudad",
                table: "tbciudades",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "str_desc_ciudad",
                table: "tbciudades");
        }
    }
}
