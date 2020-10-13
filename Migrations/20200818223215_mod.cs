using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiParquimetros.Migrations
{
    public partial class mod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dec_por_ent_total",
                table: "tbresumendiario",
                newName: "dec_por_ant_total");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dec_por_ant_total",
                table: "tbresumendiario",
                newName: "dec_por_ent_total");
        }
    }
}
