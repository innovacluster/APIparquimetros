using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class VehiculosSidConcesion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intidconcesion_id",
                table: "tbvehiculos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "intidconcesion_id",
                table: "tbvehiculos",
                nullable: false,
                defaultValue: 0);
        }
    }
}
