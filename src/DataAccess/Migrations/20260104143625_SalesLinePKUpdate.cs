using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanCart.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SalesLinePKUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId",
                table: "SalesLines");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId_Color_Size",
                table: "SalesLines",
                columns: new[] { "SalesOrderId", "ProductId", "Color", "Size" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId_Color_Size",
                table: "SalesLines");

            migrationBuilder.CreateIndex(
                name: "IX_SalesLines_SalesOrderId_ProductId",
                table: "SalesLines",
                columns: new[] { "SalesOrderId", "ProductId" },
                unique: true);
        }
    }
}
