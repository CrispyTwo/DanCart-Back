using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanCart.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PKUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_Id",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesLines_SalesOrderId",
                table: "SalesLines");

            migrationBuilder.DropIndex(
                name: "IX_Products_Id",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId",
                table: "SalesLines",
                columns: new[] { "SalesOrderId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId",
                table: "SalesLines");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_Id",
                table: "SalesOrders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesLines_SalesOrderId",
                table: "SalesLines",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true);
        }
    }
}
