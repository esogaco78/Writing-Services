using Microsoft.EntityFrameworkCore.Migrations;

namespace Tycoon.Data.Migrations
{
    public partial class ServiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_Category_CategoryId",
                table: "MenuItem");

            migrationBuilder.DropForeignKey(
                name: "FK_MenuItem_SubCategory_SubCategoryId",
                table: "MenuItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem");

            migrationBuilder.RenameTable(
                name: "MenuItem",
                newName: "Service");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_SubCategoryId",
                table: "Service",
                newName: "IX_Service_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_MenuItem_CategoryId",
                table: "Service",
                newName: "IX_Service_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Category_CategoryId",
                table: "Service",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_SubCategory_SubCategoryId",
                table: "Service",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_Category_CategoryId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_SubCategory_SubCategoryId",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "MenuItem");

            migrationBuilder.RenameIndex(
                name: "IX_Service_SubCategoryId",
                table: "MenuItem",
                newName: "IX_MenuItem_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Service_CategoryId",
                table: "MenuItem",
                newName: "IX_MenuItem_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MenuItem",
                table: "MenuItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_Category_CategoryId",
                table: "MenuItem",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItem_SubCategory_SubCategoryId",
                table: "MenuItem",
                column: "SubCategoryId",
                principalTable: "SubCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
