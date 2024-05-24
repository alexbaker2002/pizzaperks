using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace pizzaperks.Data.Migrations
{
    /// <inheritdoc />
    public partial class _004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Alterations",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PzUserId",
                table: "Carts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderModifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderNumber = table.Column<string>(type: "text", nullable: true),
                    LineItem = table.Column<int>(type: "integer", nullable: false),
                    IngredientId = table.Column<int>(type: "integer", nullable: false),
                    CostOfModification = table.Column<double>(type: "double precision", nullable: false),
                    LeaveIngredientOffProduct = table.Column<bool>(type: "boolean", nullable: false),
                    AddDoubleIngredient = table.Column<bool>(type: "boolean", nullable: false),
                    ReasonForModification = table.Column<string>(type: "text", nullable: true),
                    ModifyingUserId = table.Column<string>(type: "text", nullable: true),
                    OrderId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderModifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderModifications_AspNetUsers_ModifyingUserId",
                        column: x => x.ModifyingUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderModifications_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderModifications_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderModifications_IngredientId",
                table: "OrderModifications",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderModifications_ModifyingUserId",
                table: "OrderModifications",
                column: "ModifyingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderModifications_OrderId",
                table: "OrderModifications",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderModifications");

            migrationBuilder.DropColumn(
                name: "PzUserId",
                table: "Carts");

            migrationBuilder.AlterColumn<bool>(
                name: "Alterations",
                table: "Orders",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
