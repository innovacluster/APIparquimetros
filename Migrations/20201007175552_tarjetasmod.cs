using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class tarjetasmod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbtarjetas_tbconcesiones_intidconcesion_id",
                table: "tbtarjetas");

            migrationBuilder.AlterColumn<int>(
                name: "intidconcesion_id",
                table: "tbtarjetas",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_tbtarjetas_tbconcesiones_intidconcesion_id",
                table: "tbtarjetas",
                column: "intidconcesion_id",
                principalTable: "tbconcesiones",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbtarjetas_tbconcesiones_intidconcesion_id",
                table: "tbtarjetas");

            migrationBuilder.AlterColumn<int>(
                name: "intidconcesion_id",
                table: "tbtarjetas",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbtarjetas_tbconcesiones_intidconcesion_id",
                table: "tbtarjetas",
                column: "intidconcesion_id",
                principalTable: "tbconcesiones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
