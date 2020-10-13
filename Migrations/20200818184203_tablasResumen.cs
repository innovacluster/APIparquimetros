using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablasResumen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_andriod",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_ant_andriod",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_ant_ios",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_ios",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_por_andriod",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_sem_autos_por_ios",
                table: "tbresumensemanal",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_andriod",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_ant_andriod",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_ant_ios",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_ios",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_por_andriod",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_mes_autos_por_ios",
                table: "tbresumenmensual",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_andriod",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_ant_andriod",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_ant_ios",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_ios",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_por_andriod",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_autos_por_ios",
                table: "tbresumendiario",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "int_sem_autos_andriod",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_sem_autos_ant_andriod",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_sem_autos_ant_ios",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_sem_autos_ios",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_sem_autos_por_andriod",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_sem_autos_por_ios",
                table: "tbresumensemanal");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_andriod",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_ant_andriod",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_ant_ios",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_ios",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_por_andriod",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_mes_autos_por_ios",
                table: "tbresumenmensual");

            migrationBuilder.DropColumn(
                name: "int_autos_andriod",
                table: "tbresumendiario");

            migrationBuilder.DropColumn(
                name: "int_autos_ant_andriod",
                table: "tbresumendiario");

            migrationBuilder.DropColumn(
                name: "int_autos_ant_ios",
                table: "tbresumendiario");

            migrationBuilder.DropColumn(
                name: "int_autos_ios",
                table: "tbresumendiario");

            migrationBuilder.DropColumn(
                name: "int_autos_por_andriod",
                table: "tbresumendiario");

            migrationBuilder.DropColumn(
                name: "int_autos_por_ios",
                table: "tbresumendiario");
        }
    }
}
