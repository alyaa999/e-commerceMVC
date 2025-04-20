using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using e_commerce.Infrastructure.Entites;
using e_commerce.Domain.Entites;


public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" },
            new Category { Id = 3, Name = "Home & Kitchen" }
        );

        // Seed SubCategories
        modelBuilder.Entity<SubCategory>().HasData(
            new SubCategory { Id = 3, Name = "Headphones", CategoryId = 1 },
            new SubCategory { Id = 4, Name = "Smartphones", CategoryId = 1 },
            new SubCategory { Id = 5, Name = "Men's T-Shirts", CategoryId = 2 },
            new SubCategory { Id = 6, Name = "Dresses", CategoryId = 2 },
            new SubCategory { Id = 7, Name = "Cookware", CategoryId = 3 },
            new SubCategory { Id = 8, Name = "Blenders", CategoryId = 3 }
        );

        // Products
        // Products with Tags and Sellers
        modelBuilder.Entity<Product>().HasData(
            // Product 1: Wireless Noise-Cancelling Headphones
            new Product
            {
                Id = 1,
                Code = "PROD-ELEC-001",
                IsApproved = true,
                Name = "Wireless Noise-Cancelling Headphones",
                Brand = "SoundMaster",
                Price = 299.99m,
                Desc = "Premium over-ear headphones with active noise cancellation and 30-hour battery life",
                Discount = 0.00m,
                Stock = 50,
                SubCategoryId = 3,
                SellerId = 1 // Seller association
            },
            // Product 2: Flagship Smartphone 2023
            new Product
            {
                Id = 2,
                Code = "PROD-ELEC-002",
                IsApproved = true,
                Name = "Flagship Smartphone 2023",
                Brand = "TechGiant",
                Price = 999.99m,
                Desc = "Latest model with 6.7\" AMOLED display and triple camera system",
                Discount = 0.00m,
                Stock = 30,
                SubCategoryId = 4,
                SellerId = 2 // Seller association
            },
            // Product 3: Premium Cotton T-Shirt
            new Product
            {
                Id = 3,
                Code = "PROD-CLOTH-001",
                IsApproved = true,
                Name = "Premium Cotton T-Shirt",
                Brand = "UrbanWear",
                Price = 29.99m,
                Desc = "100% organic cotton crew neck t-shirt for men",
                Discount = 10.00m,
                Stock = 200,
                SubCategoryId = 5,
                SellerId = 3 // Seller association
            },
            // Product 4: Summer Floral Dress
            new Product
            {
                Id = 4,
                Code = "PROD-CLOTH-002",
                IsApproved = true,
                Name = "Summer Floral Dress",
                Brand = "FashionStyle",
                Price = 59.99m,
                Desc = "Lightweight floral dress with adjustable straps",
                Discount = 0.00m,
                Stock = 75,
                SubCategoryId = 6,
                SellerId = 4 // Seller association
            },
            // Product 5: Non-Stick Cookware Set
            new Product
            {
                Id = 5,
                Code = "PROD-HOME-001",
                IsApproved = true,
                Name = "Non-Stick Cookware Set",
                Brand = "KitchenPro",
                Price = 149.99m,
                Desc = "10-piece non-stick cookware set with glass lids",
                Discount = 0.00m,
                Stock = 40,
                SubCategoryId = 7,
                SellerId = 5 // Seller association
            },
            // Product 6: Professional Blender
            new Product
            {
                Id = 6,
                Code = "PROD-HOME-002",
                IsApproved = true,
                Name = "Professional Blender",
                Brand = "BlendTech",
                Price = 89.99m,
                Desc = "High-powered blender with 1200W motor and 8-speed control",
                Discount = 15.00m,
                Stock = 25,
                SubCategoryId = 8,
                SellerId = 1 // Seller association
            }
        );

        
        // Tager seeding
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "feature" },
            new Tag { Id = 2, Name = "Popular" },
            new Tag { Id = 3, Name = "New" },
            new Tag { Id = 4, Name = "BestSeller" },
            new Tag { Id = 5, Name = "HotRelease" },
            new Tag { Id = 6, Name = "BestDeal" },
            new Tag{ Id = 7, Name = "TopSelling" },
            new Tag{ Id = 8, Name = "Trending" }
        );

        // Product Images
        modelBuilder.Entity<ProductImage>().HasData(
            new ProductImage { Id = 1, ProductId = 1, ImageUrl = "product-1-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 2, ProductId = 1, ImageUrl = "product-1-2.jpg", IsPrimary = false, DisplayOrder = 0 },
            new ProductImage { Id = 3, ProductId = 2, ImageUrl = "product-2-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 4, ProductId = 2, ImageUrl = "product-2-2.jpg", IsPrimary = false, DisplayOrder = 0 },
            new ProductImage { Id = 5, ProductId = 3, ImageUrl = "product-3-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 6, ProductId = 3, ImageUrl = "product-3-2.jpg", IsPrimary = false, DisplayOrder = 0 },
            new ProductImage { Id = 7, ProductId = 4, ImageUrl = "product-4-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 8, ProductId = 4, ImageUrl = "product-4-2.jpg", IsPrimary = false, DisplayOrder = 0 },
            new ProductImage { Id = 9, ProductId = 5, ImageUrl = "product-5-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 10, ProductId = 5, ImageUrl = "product-5-2.jpg", IsPrimary = false, DisplayOrder = 0 },
            new ProductImage { Id = 11, ProductId = 6, ImageUrl = "product-6-1.jpg", IsPrimary = true, DisplayOrder = 0 },
            new ProductImage { Id = 12, ProductId = 6, ImageUrl = "product-6-2.jpg", IsPrimary = false, DisplayOrder = 0 }
        );
        // Seeding Reviews using Customer table int IDs
        modelBuilder.Entity<Review>().HasData(
            // Product 1
            new Review { Id = 1, ProductId = 1, CustomerId = 1, Rating = 5, Comment = "Amazing sound quality and battery life!", ReviewDate = new DateTime(2024, 5, 15) },
            new Review { Id = 2, ProductId = 1, CustomerId = 2, Rating = 4, Comment = "Comfortable to wear for hours.", ReviewDate = new DateTime(2024, 5, 18) },
            new Review { Id = 3, ProductId = 1, CustomerId = 3, Rating = 5, Comment = "Love the ANC feature!", ReviewDate = new DateTime(2024, 5, 20) },
            new Review { Id = 4, ProductId = 1, CustomerId = 4, Rating = 3, Comment = "Good, but a bit pricey.", ReviewDate = new DateTime(2024, 5, 21) },
            new Review { Id = 5, ProductId = 1, CustomerId = 5, Rating = 4, Comment = "Battery lasts all day long.", ReviewDate = new DateTime(2024, 5, 22) },

            // Product 2
            new Review { Id = 6, ProductId = 2, CustomerId = 1, Rating = 5, Comment = "Blazing fast and a great display.", ReviewDate = new DateTime(2024, 6, 10) },
            new Review { Id = 7, ProductId = 2, CustomerId = 2, Rating = 4, Comment = "Camera is top-notch.", ReviewDate = new DateTime(2024, 6, 11) },
            new Review { Id = 8, ProductId = 2, CustomerId = 3, Rating = 4, Comment = "Great battery life.", ReviewDate = new DateTime(2024, 6, 13) },
            new Review { Id = 9, ProductId = 2, CustomerId = 4, Rating = 3, Comment = "Performance is smooth, but gets hot.", ReviewDate = new DateTime(2024, 6, 14) },
            new Review { Id = 10, ProductId = 2, CustomerId = 5, Rating = 5, Comment = "Definitely worth the upgrade.", ReviewDate = new DateTime(2024, 6, 15) },

            // Product 3
            new Review { Id = 11, ProductId = 3, CustomerId = 1, Rating = 5, Comment = "Very comfortable and stylish.", ReviewDate = new DateTime(2024, 6, 18) },
            new Review { Id = 12, ProductId = 3, CustomerId = 2, Rating = 4, Comment = "Color doesn't fade after wash.", ReviewDate = new DateTime(2024, 6, 20) },
            new Review { Id = 13, ProductId = 3, CustomerId = 3, Rating = 4, Comment = "Fits as expected.", ReviewDate = new DateTime(2024, 6, 21) },
            new Review { Id = 14, ProductId = 3, CustomerId = 4, Rating = 3, Comment = "Material is decent.", ReviewDate = new DateTime(2024, 6, 22) },
            new Review { Id = 15, ProductId = 3, CustomerId = 5, Rating = 5, Comment = "Great value for money!", ReviewDate = new DateTime(2024, 6, 23) },

            // Product 4
            new Review { Id = 16, ProductId = 4, CustomerId = 1, Rating = 4, Comment = "Beautiful dress, perfect fit!", ReviewDate = new DateTime(2024, 7, 2) },
            new Review { Id = 17, ProductId = 4, CustomerId = 2, Rating = 3, Comment = "A bit short, but cute.", ReviewDate = new DateTime(2024, 7, 3) },
            new Review { Id = 18, ProductId = 4, CustomerId = 3, Rating = 5, Comment = "Love the floral pattern!", ReviewDate = new DateTime(2024, 7, 4) },
            new Review { Id = 19, ProductId = 4, CustomerId = 4, Rating = 4, Comment = "Light and breathable.", ReviewDate = new DateTime(2024, 7, 5) },
            new Review { Id = 20, ProductId = 4, CustomerId = 5, Rating = 4, Comment = "Straps are adjustable, nice touch.", ReviewDate = new DateTime(2024, 7, 6) },

            // Product 5
            new Review { Id = 21, ProductId = 5, CustomerId = 1, Rating = 5, Comment = "Excellent quality cookware.", ReviewDate = new DateTime(2024, 7, 12) },
            new Review { Id = 22, ProductId = 5, CustomerId = 2, Rating = 4, Comment = "Non-stick works as promised.", ReviewDate = new DateTime(2024, 7, 13) },
            new Review { Id = 23, ProductId = 5, CustomerId = 3, Rating = 4, Comment = "Easy to clean and durable.", ReviewDate = new DateTime(2024, 7, 14) },
            new Review { Id = 24, ProductId = 5, CustomerId = 4, Rating = 3, Comment = "Handles get a little hot.", ReviewDate = new DateTime(2024, 7, 15) },
            new Review { Id = 25, ProductId = 5, CustomerId = 5, Rating = 5, Comment = "Perfect for everyday use.", ReviewDate = new DateTime(2024, 7, 16) },

            // Product 6
            new Review { Id = 26, ProductId = 6, CustomerId = 1, Rating = 5, Comment = "Very powerful and fast blending.", ReviewDate = new DateTime(2024, 7, 19) },
            new Review { Id = 27, ProductId = 6, CustomerId = 2, Rating = 4, Comment = "Crushes ice like a champ!", ReviewDate = new DateTime(2024, 7, 20) },
            new Review { Id = 28, ProductId = 6, CustomerId = 3, Rating = 4, Comment = "Great for smoothies.", ReviewDate = new DateTime(2024, 7, 21) },
            new Review { Id = 29, ProductId = 6, CustomerId = 4, Rating = 3, Comment = "A bit noisy but works well.", ReviewDate = new DateTime(2024, 7, 22) },
            new Review { Id = 30, ProductId = 6, CustomerId = 5, Rating = 5, Comment = "Blends in seconds, love it!", ReviewDate = new DateTime(2024, 7, 23) }
        );


    }
}
