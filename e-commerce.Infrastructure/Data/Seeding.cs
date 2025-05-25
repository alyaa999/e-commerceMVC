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
     

      
    }
}
