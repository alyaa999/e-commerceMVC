using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Clothing" },
                    { 3, "Home & Kitchen" }
                });

            migrationBuilder.InsertData(
                table: "Sub_Category",
                columns: new[] { "ID", "Category_ID", "Name" },
                values: new object[,]
                {
                    { 3, 1, "Headphones" },
                    { 4, 1, "Smartphones" },
                    { 5, 2, "Men's T-Shirts" },
                    { 6, 2, "Dresses" },
                    { 7, 3, "Cookware" },
                    { 8, 3, "Blenders" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ID", "Brand", "Code", "CreatedAt", "DESC", "Discount", "IsActive", "IsApproved", "Name", "Price", "Seller_ID", "Stock", "Sub_Category_ID", "Tag", "TagId" },
                values: new object[,]
                {
                    { 1, "SoundMaster", "PROD-ELEC-001", null, "Premium over-ear headphones with active noise cancellation and 30-hour battery life", 0.00m, false, true, "Wireless Noise-Cancelling Headphones", 299.99m, null, 50, 3, null, null },
                    { 2, "TechGiant", "PROD-ELEC-002", null, "Latest model with 6.7\" AMOLED display and triple camera system", 0.00m, false, true, "Flagship Smartphone 2023", 999.99m, null, 30, 4, null, null },
                    { 3, "UrbanWear", "PROD-CLOTH-001", null, "100% organic cotton crew neck t-shirt for men", 10.00m, false, true, "Premium Cotton T-Shirt", 29.99m, null, 200, 5, null, null },
                    { 4, "FashionStyle", "PROD-CLOTH-002", null, "Lightweight floral dress with adjustable straps", 0.00m, false, true, "Summer Floral Dress", 59.99m, null, 75, 6, null, null },
                    { 5, "KitchenPro", "PROD-HOME-001", null, "10-piece non-stick cookware set with glass lids", 0.00m, false, true, "Non-Stick Cookware Set", 149.99m, null, 40, 7, null, null },
                    { 6, "BlendTech", "PROD-HOME-002", null, "High-powered blender with 1200W motor and 8-speed control", 15.00m, false, true, "Professional Blender", 89.99m, null, 25, 8, null, null }
                });

            migrationBuilder.InsertData(
                table: "Product_Image",
                columns: new[] { "ID", "Display_Order", "Image_URL", "Is_Primary", "Product_ID" },
                values: new object[,]
                {
                    { 1, 0, "product-1-1.jpg", true, 1 },
                    { 2, 0, "product-1-2.jpg", false, 1 },
                    { 3, 0, "product-2-1.jpg", true, 2 },
                    { 4, 0, "product-2-2.jpg", false, 2 },
                    { 5, 0, "product-3-1.jpg", true, 3 },
                    { 6, 0, "product-3-2.jpg", false, 3 },
                    { 7, 0, "product-4-1.jpg", true, 4 },
                    { 8, 0, "product-4-2.jpg", false, 4 },
                    { 9, 0, "product-5-1.jpg", true, 5 },
                    { 10, 0, "product-5-2.jpg", false, 5 },
                    { 11, 0, "product-6-1.jpg", true, 6 },
                    { 12, 0, "product-6-2.jpg", false, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Product_Image",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Sub_Category",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
