using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace WebApiParquimetros.Migrations
{
    public partial class tbCuidadesPara : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "str_descrip_us_admin",
                table: "tbparametros",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "tbcatciudades",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    str_ciudad = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbcatciudades", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbcatciudades");

            migrationBuilder.DropColumn(
                name: "str_descrip_us_admin",
                table: "tbparametros");
        }
    }
}
