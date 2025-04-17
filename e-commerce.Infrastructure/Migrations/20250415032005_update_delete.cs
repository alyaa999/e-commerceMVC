using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Order_Pro__Order__5441852A",
                table: "Order_Product");

            migrationBuilder.AddForeignKey(
                name: "FK__Order_Pro__Order__5441852A",
                table: "Order_Product",
                column: "Order_ID",
                principalTable: "Order",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Order_Pro__Order__5441852A",
                table: "Order_Product");

            migrationBuilder.AddForeignKey(
                name: "FK__Order_Pro__Order__5441852A",
                table: "Order_Product",
                column: "Order_ID",
                principalTable: "Order",
                principalColumn: "ID");
        }
    }
}
