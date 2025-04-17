using Azure;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.Domain.DTOS;
namespace e_commerce.Infrastructure.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ECommerceDBContext _context;

        public HomeRepository(ECommerceDBContext context)
        {
            _context = context;
        }

        public IQueryable<Product> GetProducts(ShopDTO shopDTO)
        {
            IQueryable<Product> queryable = _context.Products;
             if(shopDTO.Name!=null)
             {
                queryable.Where(p => p.Name.Contains(shopDTO.Name));
             }
           
            if (shopDTO.CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategory.CategoryId == shopDTO.CategoryId.Value);
            }

            if (shopDTO.SubCategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategoryId == shopDTO.SubCategoryId.Value);
            }
            if (shopDTO.BrandFilter != null && shopDTO.BrandFilter.Any())
            {
                // Create OR condition for brands (products matching ANY of the selected brands)
                queryable = queryable.Where(p => shopDTO.BrandFilter.Contains(p.Brand));
            }

            if (shopDTO.PriceFilter.HasValue)
            {
                queryable = queryable.Where(p => p.Price >= 0 && p.Price <= shopDTO.PriceFilter.Value);
            }

            if (shopDTO.TagFilter != null && shopDTO.TagFilter.Any())
            {
               
                queryable = queryable.Where(p =>
                {
                    return p.Tag.Any(pt => shopDTO.TagFilter.Contains((Tager)pt));
                });
            }
            return queryable.Include(i => i.SubCategory).Include(i=>i.ProductImages).Include(i=>i.Reviews);
        }
       


    
        public List<Category> GetCategories()
        {
            return _context.Categories.Include(i=>i.SubCategories)
                .ToList();

        }
        public List<string> GetBrands()
        {
            return _context.Products.Select(i => i.Brand).Distinct().ToList();
        }
       
        public List<Tager> GetTagers()
        {
            return  Enum.GetValues(typeof(Tager))
                             .Cast<Tager>()
                             .ToList();

        }
        public IQueryable<Product> GetProductsByCategory(int? CategoryId, int? SubCategoryId)
        {
            IQueryable<Product> queryable = _context.Products;

            if (CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategory.CategoryId == CategoryId.Value);
            }

            if (SubCategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategoryId == SubCategoryId.Value);
            }
            return queryable.Include(i => i.SubCategory).Include(i => i.ProductImages).Include(i => i.Reviews);

        }
        public IQueryable<Product> GetProductsByTag(int? Tager)
        {
            return _context.Products
                .Include(i=>i.SubCategory.Category)
                .Include(i=>i.ProductImages).Where(i => i.Tag == (Tager)Tager);
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.Include(i => i.ProductImages).Include(i=>i.SubCategory.Category).Include(i=>i.Reviews).FirstOrDefault(i => i.Id == id);
        }

       
    }
}
