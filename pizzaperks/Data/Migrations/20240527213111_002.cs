using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pizzaperks.Data.Migrations
{
    /// <inheritdoc />
    public partial class _002 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderModifications_Ingredients_IngredientId",
                table: "OrderModifications");

            migrationBuilder.DropIndex(
                name: "IX_OrderModifications_IngredientId",
                table: "OrderModifications");

            migrationBuilder.DropColumn(
                name: "AddDoubleIngredient",
                table: "OrderModifications");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "OrderModifications");

            migrationBuilder.DropColumn(
                name: "LeaveIngredientOffProduct",
                table: "OrderModifications");

            migrationBuilder.DropColumn(
                name: "LineItem",
                table: "OrderModifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AddDoubleIngredient",
                table: "OrderModifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "OrderModifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "LeaveIngredientOffProduct",
                table: "OrderModifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LineItem",
                table: "OrderModifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderModifications_IngredientId",
                table: "OrderModifications",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderModifications_Ingredients_IngredientId",
                table: "OrderModifications",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
