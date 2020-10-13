using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class vehiculosmarca : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "str_placas",
                table: "tbvehiculos",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "str_marca",
                table: "tbvehiculos",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "str_marca",
                table: "tbvehiculos");

            migrationBuilder.AlterColumn<string>(
                name: "str_placas",
                table: "tbvehiculos",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
