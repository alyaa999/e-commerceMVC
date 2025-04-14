using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addsellercode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerCode",
                table: "Seller",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tag",
                table: "Product",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellerCode",
                table: "Seller");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Product");
        }
    }
}
