using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class Parametroscomision3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PorcentajeComisionRecarga",
                table: "tbparametros",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
