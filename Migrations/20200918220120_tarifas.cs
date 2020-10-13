using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tarifas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dtmVigencia",
                table: "tbtarifas");

            migrationBuilder.RenameColumn(
                name: "fltTarifa",
                table: "tbtarifas",
                newName: "flt_tarifa_min");

            migrationBuilder.RenameColumn(
                name: "fltImpuestos",
                table: "tbtarifas",
                newName: "flt_tarifa_max");

            migrationBuilder.RenameColumn(
                name: "fltIVA",
                table: "tbtarifas",
                newName: "flt_tarifa_intervalo");

            migrationBuilder.AddColumn<bool>(
                name: "bool_cobro_fraccio",
                table: "tbtarifas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "int_intervalo_minutos",
                table: "tbtarifas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_tiempo_maximo",
                table: "tbtarifas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "int_tiempo_minimo",
                table: "tbtarifas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "intidconcesion_id",
                table: "tbtarifas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "str_tipo",
                table: "tbtarifas",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbtarifas_intidconcesion_id",
                table: "tbtarifas",
                column: "intidconcesion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_tbtarifas_tbconcesiones_intidconcesion_id",
                table: "tbtarifas",
                column: "intidconcesion_id",
                principalTable: "tbconcesiones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbtarifas_tbconcesiones_intidconcesion_id",
                table: "tbtarifas");

            migrationBuilder.DropIndex(
                name: "IX_tbtarifas_intidconcesion_id",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "bool_cobro_fraccio",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "int_intervalo_minutos",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "int_tiempo_maximo",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "int_tiempo_minimo",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "intidconcesion_id",
                table: "tbtarifas");

            migrationBuilder.DropColumn(
                name: "str_tipo",
                table: "tbtarifas");

            migrationBuilder.RenameColumn(
                name: "flt_tarifa_min",
                table: "tbtarifas",
                newName: "fltTarifa");

            migrationBuilder.RenameColumn(
                name: "flt_tarifa_max",
                table: "tbtarifas",
                newName: "fltImpuestos");

            migrationBuilder.RenameColumn(
                name: "flt_tarifa_intervalo",
                table: "tbtarifas",
                newName: "fltIVA");

            migrationBuilder.AddColumn<DateTime>(
                name: "dtmVigencia",
                table: "tbtarifas",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
