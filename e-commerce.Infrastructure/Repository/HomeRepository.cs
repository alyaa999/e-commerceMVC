using Azure;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ECommerceDBContext _context;

        public HomeRepository(ECommerceDBContext context)
        {
            _context = context;

        }

        public IQueryable<Product> GetProductsByCategory(int? CategoryId, int? SubCategoryId)
        {
            IQueryable<Product> queryable = _context.Products.Include(p => p.SubCategory);

            if (CategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategory.CategoryId == CategoryId.Value);
            }

            if (SubCategoryId.HasValue)
            {
                queryable = queryable.Where(p => p.SubCategoryId == SubCategoryId.Value);
            }

            return queryable.Include(i => i.SubCategory).Include(i=>i.ProductImages);
        }
        public List<Product> GetProductsByTag(int tagId)
        {
            Tager tag = (Tager)tagId;
            return _context.Products
                .Where(p => p.Tag == tag)
                .ToList();
        }


        public List<Product> GetProductsByName(string name)
        {
            return _context.Products
                .Where(p => p.Name.Contains(name))
                .ToList();
        }
        public List<Category> GetCategories()
        {
            return _context.Categories.Include(i=>i.SubCategories)
                .ToList();

        }
       
        public Product? GetProductById(int id)
        {
            return _context.Products.Include(i => i.ProductImages).Include(i=>i.SubCategory.Category).FirstOrDefault(i => i.Id == id);
        }
    }
}
