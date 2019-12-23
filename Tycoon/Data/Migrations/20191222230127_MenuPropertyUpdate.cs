using Microsoft.EntityFrameworkCore.Migrations;

namespace Tycoon.Data.Migrations
{
    public partial class MenuPropertyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Demand",
                table: "MenuItem");

            migrationBuilder.DropColumn(
                name: "Descriptio",
                table: "MenuItem");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MenuItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Popularity",
                table: "MenuItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MenuItem");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "MenuItem");

            migrationBuilder.AddColumn<string>(
                name: "Demand",
                table: "MenuItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descriptio",
                table: "MenuItem",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
