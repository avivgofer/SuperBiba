using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoreApp.Migrations
{
    public partial class _2_MODELS_ADDED : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailsOrderID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    userName = table.Column<string>(nullable: true),
                    orderTime = table.Column<DateTime>(nullable: false),
                    total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "StorageProducts",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(nullable: true),
                    LastOrder = table.Column<DateTime>(nullable: false),
                    SupplierID = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageProducts", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_StorageProducts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderDetailsOrderID",
                table: "Products",
                column: "OrderDetailsOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_StorageProducts_SupplierID",
                table: "StorageProducts",
                column: "SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailsOrderID",
                table: "Products",
                column: "OrderDetailsOrderID",
                principalTable: "OrderDetails",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderDetails_OrderDetailsOrderID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "StorageProducts");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderDetailsOrderID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderDetailsOrderID",
                table: "Products");
        }
    }
}
