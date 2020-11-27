using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class alterCiudades3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "int_id_ciudad",
                table: "tbciudades",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbciudades_int_id_ciudad",
                table: "tbciudades",
                column: "int_id_ciudad");

            migrationBuilder.AddForeignKey(
                name: "FK_tbciudades_tbcatciudades_int_id_ciudad",
                table: "tbciudades",
                column: "int_id_ciudad",
                principalTable: "tbcatciudades",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbciudades_tbcatciudades_int_id_ciudad",
                table: "tbciudades");

            migrationBuilder.DropIndex(
                name: "IX_tbciudades_int_id_ciudad",
                table: "tbciudades");

            migrationBuilder.DropColumn(
                name: "int_id_ciudad",
                table: "tbciudades");
        }
    }
}
