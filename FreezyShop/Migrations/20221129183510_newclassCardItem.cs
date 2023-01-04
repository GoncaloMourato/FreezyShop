using Microsoft.EntityFrameworkCore.Migrations;

namespace FreezyShop.Migrations
{
    public partial class newclassCardItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingInfoS_AspNetUsers_UserId",
                table: "ShippingInfoS");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingInfoS",
                table: "ShippingInfoS");

            migrationBuilder.RenameTable(
                name: "ShippingInfoS",
                newName: "ShippingInfos");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingInfoS_UserId",
                table: "ShippingInfos",
                newName: "IX_ShippingInfos_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingInfos",
                table: "ShippingInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingInfos_AspNetUsers_UserId",
                table: "ShippingInfos",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingInfos_AspNetUsers_UserId",
                table: "ShippingInfos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingInfos",
                table: "ShippingInfos");

            migrationBuilder.RenameTable(
                name: "ShippingInfos",
                newName: "ShippingInfoS");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingInfos_UserId",
                table: "ShippingInfoS",
                newName: "IX_ShippingInfoS_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingInfoS",
                table: "ShippingInfoS",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingInfoS_AspNetUsers_UserId",
                table: "ShippingInfoS",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
