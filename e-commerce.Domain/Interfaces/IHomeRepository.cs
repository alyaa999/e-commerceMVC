using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Repository
{
    public interface IHomeRepository
    {
        public IQueryable<Product> GetProductsByCategory(int? CategoryId, int? SubCategoryId);
        public List<Category> GetCategories();
        public Product? GetProductById(int id);


    }
}
