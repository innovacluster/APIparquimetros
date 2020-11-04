using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class SaldosMonto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<double>(
                name: "flt_monto",
                table: "tbsaldo",
                nullable: false,
                defaultValue: 0.0);

         
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flt_monto",
                table: "tbsaldo");

        
         
        }
    }
}
