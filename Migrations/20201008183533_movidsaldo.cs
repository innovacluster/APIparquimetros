using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class movidsaldo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbmovimientos_tbsaldo_int_id_saldo_id",
                table: "tbmovimientos");

            migrationBuilder.AlterColumn<int>(
                name: "int_id_saldo_id",
                table: "tbmovimientos",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_tbmovimientos_tbsaldo_int_id_saldo_id",
                table: "tbmovimientos",
                column: "int_id_saldo_id",
                principalTable: "tbsaldo",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbmovimientos_tbsaldo_int_id_saldo_id",
                table: "tbmovimientos");

            migrationBuilder.AlterColumn<int>(
                name: "int_id_saldo_id",
                table: "tbmovimientos",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbmovimientos_tbsaldo_int_id_saldo_id",
                table: "tbmovimientos",
                column: "int_id_saldo_id",
                principalTable: "tbsaldo",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
