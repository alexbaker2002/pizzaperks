using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pizzaperks.Data.Migrations
{
    /// <inheritdoc />
    public partial class _003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "Products",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId1",
                table: "Products",
                column: "OrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId1",
                table: "Products",
                column: "OrderId1",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "Products");
        }
    }
}
