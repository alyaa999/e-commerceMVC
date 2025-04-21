using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace e_commerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedingData : Migration
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
                table: "Tager",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "feature" },
                    { 2, "Popular" },
                    { 3, "New" },
                    { 4, "BestSeller" },
                    { 5, "HotRelease" },
                    { 6, "BestDeal" },
                    { 7, "TopSelling" },
                    { 8, "Trending" }
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
                    { 1, "SoundMaster", "PROD-ELEC-001", null, "Premium over-ear headphones with active noise cancellation and 30-hour battery life", 0.00m, false, true, "Wireless Noise-Cancelling Headphones", 299.99m, 1, 50, 3, null, null },
                    { 2, "TechGiant", "PROD-ELEC-002", null, "Latest model with 6.7\" AMOLED display and triple camera system", 0.00m, false, true, "Flagship Smartphone 2023", 999.99m, 2, 30, 4, null, null },
                    { 3, "UrbanWear", "PROD-CLOTH-001", null, "100% organic cotton crew neck t-shirt for men", 10.00m, false, true, "Premium Cotton T-Shirt", 29.99m, 3, 200, 5, null, null },
                    { 4, "FashionStyle", "PROD-CLOTH-002", null, "Lightweight floral dress with adjustable straps", 0.00m, false, true, "Summer Floral Dress", 59.99m, 4, 75, 6, null, null },
                    { 5, "KitchenPro", "PROD-HOME-001", null, "10-piece non-stick cookware set with glass lids", 0.00m, false, true, "Non-Stick Cookware Set", 149.99m, 5, 40, 7, null, null },
                    { 6, "BlendTech", "PROD-HOME-002", null, "High-powered blender with 1200W motor and 8-speed control", 15.00m, false, true, "Professional Blender", 89.99m, 1, 25, 8, null, null }
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

            migrationBuilder.InsertData(
                table: "Review",
                columns: new[] { "ID", "Comment", "Customer_ID", "Product_ID", "Rating", "Review_Date" },
                values: new object[,]
                {
                    { 1, "Amazing sound quality and battery life!", 1, 1, 5, new DateTime(2024, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Comfortable to wear for hours.", 2, 1, 4, new DateTime(2024, 5, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Love the ANC feature!", 3, 1, 5, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Good, but a bit pricey.", 4, 1, 3, new DateTime(2024, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Battery lasts all day long.", 5, 1, 4, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "Blazing fast and a great display.", 1, 2, 5, new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "Camera is top-notch.", 2, 2, 4, new DateTime(2024, 6, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "Great battery life.", 3, 2, 4, new DateTime(2024, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "Performance is smooth, but gets hot.", 4, 2, 3, new DateTime(2024, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "Definitely worth the upgrade.", 5, 2, 5, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "Very comfortable and stylish.", 1, 3, 5, new DateTime(2024, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "Color doesn't fade after wash.", 2, 3, 4, new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "Fits as expected.", 3, 3, 4, new DateTime(2024, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "Material is decent.", 4, 3, 3, new DateTime(2024, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "Great value for money!", 5, 3, 5, new DateTime(2024, 6, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "Beautiful dress, perfect fit!", 1, 4, 4, new DateTime(2024, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, "A bit short, but cute.", 2, 4, 3, new DateTime(2024, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, "Love the floral pattern!", 3, 4, 5, new DateTime(2024, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, "Light and breathable.", 4, 4, 4, new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, "Straps are adjustable, nice touch.", 5, 4, 4, new DateTime(2024, 7, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, "Excellent quality cookware.", 1, 5, 5, new DateTime(2024, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, "Non-stick works as promised.", 2, 5, 4, new DateTime(2024, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, "Easy to clean and durable.", 3, 5, 4, new DateTime(2024, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, "Handles get a little hot.", 4, 5, 3, new DateTime(2024, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 25, "Perfect for everyday use.", 5, 5, 5, new DateTime(2024, 7, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 26, "Very powerful and fast blending.", 1, 6, 5, new DateTime(2024, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 27, "Crushes ice like a champ!", 2, 6, 4, new DateTime(2024, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 28, "Great for smoothies.", 3, 6, 4, new DateTime(2024, 7, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 29, "A bit noisy but works well.", 4, 6, 3, new DateTime(2024, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 30, "Blends in seconds, love it!", 5, 6, 5, new DateTime(2024, 7, 23, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                table: "Review",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Review",
                keyColumn: "ID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tager",
                keyColumn: "Id",
                keyValue: 8);

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
