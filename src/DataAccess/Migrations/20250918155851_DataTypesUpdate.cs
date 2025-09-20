using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanCart.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class DataTypesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "SalesOrders",
                newName: "Phone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "SalesOrders",
                newName: "PhoneNumber");
        }
    }
}
