using Microsoft.EntityFrameworkCore.Migrations;

namespace Tycoon.Data.Migrations
{
    public partial class CouponPropertyChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "picture",
                table: "Coupon",
                newName: "Picture");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Picture",
                table: "Coupon",
                newName: "picture");
        }
    }
}
