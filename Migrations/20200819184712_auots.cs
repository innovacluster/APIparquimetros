using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class auots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "int_sem_total_autos",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_total_autos",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_total_autos",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "int_sem_total_autos",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_mes_total_autos",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_total_autos",
                table: "tbresumendiario");
        }
    }
}
