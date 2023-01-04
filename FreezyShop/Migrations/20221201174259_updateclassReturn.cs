using Microsoft.EntityFrameworkCore.Migrations;

namespace FreezyShop.Migrations
{
    public partial class updateclassReturn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Motivo",
                table: "Returns",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "Descrição",
                table: "Returns",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Returns",
                newName: "Motivo");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Returns",
                newName: "Descrição");
        }
    }
}
