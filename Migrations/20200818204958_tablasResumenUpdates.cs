using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tablasResumenUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_total_ant",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_total",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_por_total",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_por_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_por_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_ant_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_ant_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_sem_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_total_ant",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_total",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_por_total",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_por_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_por_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_ant_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_ant_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_mes_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_total_ant",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_total",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_por_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_por_ent_total",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_por_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_ant_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_ant_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "dec_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_total_ant",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_total",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_por_total",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_por_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_por_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_ant_ios",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_ant_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_sem_andriod",
                table: "tbresumensemanal",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_total_ant",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_total",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_por_total",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_por_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_por_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_ant_ios",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_ant_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_mes_andriod",
                table: "tbresumenmensual",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_total_ant",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_total",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_por_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_por_ent_total",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_por_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_ant_ios",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_ant_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "dec_andriod",
                table: "tbresumendiario",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
