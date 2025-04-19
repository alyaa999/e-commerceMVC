using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class createtagtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tager",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tager", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_TagId",
                table: "Product",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Tager_TagId",
                table: "Product",
                column: "TagId",
                principalTable: "Tager",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Tager_TagId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Tager");

            migrationBuilder.DropIndex(
                name: "IX_Product_TagId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Product");
        }
    }
}
