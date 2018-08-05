using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class orderDetailsv20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cost",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "OrderDetails",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "total",
                table: "OrderDetails",
                newName: "Total");

            migrationBuilder.RenameColumn(
                name: "orderTime",
                table: "OrderDetails",
                newName: "OrderTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "cost");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "OrderDetails",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "OrderDetails",
                newName: "total");

            migrationBuilder.RenameColumn(
                name: "OrderTime",
                table: "OrderDetails",
                newName: "orderTime");
        }
    }
}
