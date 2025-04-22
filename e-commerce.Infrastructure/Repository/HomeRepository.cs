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
using e_commerce.Domain.Entites;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
            IQueryable<Product> queryable = _context.Products.Where(i=>i.IsApproved);

            if (!string.IsNullOrWhiteSpace(shopDTO.Name))
            {
                queryable = queryable.Where(p => p.Name.ToLower().Contains(shopDTO.Name.ToLower()));
            }

            if (shopDTO.CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategory.CategoryId == shopDTO.CategoryId.Value);
            }

            if (shopDTO.SubCategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategoryId == shopDTO.SubCategoryId.Value);
            }

            if (shopDTO.BrandFilter?.Any() == true)
            {
                queryable = queryable.Where(p => shopDTO.BrandFilter.Contains(p.Brand));
            }

            if (shopDTO.PriceFilter.HasValue)
            {
                queryable = queryable.Where(p => p.Price >= 0 && p.Price <= shopDTO.PriceFilter.Value);
            }
            if (shopDTO.TagFilter?.Any() == true)
            {
                queryable = queryable.Where(p => shopDTO.TagFilter.Contains(p.TagId));
            }




            return queryable
                .Include(p => p.SubCategory)
                .Include(p => p.ProductImages)
                .Include(p => p.Reviews)
                .Include(p => p.TagObj);
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
       
        public List<Tag> GetTagers()
        {
            return _context.Tags.ToList();

        }
        public IQueryable<Product> GetProductsByCategory(int? CategoryId, int? SubCategoryId)
        {
            IQueryable<Product> queryable = _context.Products.Where(i => i.IsApproved);

            if (CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategory.CategoryId == CategoryId.Value);
            }

            if (SubCategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategoryId == SubCategoryId.Value);
            }
            return queryable.Include(i => i.SubCategory).Include(i => i.ProductImages).Include(i => i.Reviews).Include(i=>i.TagObj);

        }
        public IQueryable<Product> GetProductsByTag(int? Tager)
        {
            return _context.Products.Where(i=>i.IsApproved)
                .Include(i=>i.SubCategory.Category)
                .Include(i => i.Reviews)
                .Include(i => i.TagObj)
                .Include(i=>i.ProductImages).Where(i => i.TagId == Tager);
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.Include(i => i.ProductImages).Include(i=>i.SubCategory.Category).Include(i=>i.Reviews).Include(i=>i.TagObj).FirstOrDefault(i => i.Id == id);
        }

       
    }
}
