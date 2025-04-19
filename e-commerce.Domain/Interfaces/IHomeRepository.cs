using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce.Domain.DTOS;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Repository
{
    public interface IHomeRepository
    {
        public IQueryable<Product> GetProducts(ShopDTO shopDTO);
        public List<Category> GetCategories();
        public Product? GetProductById(int id);
        public List<string> GetBrands();
        public List<Tag> GetTagers();

        public IQueryable<Product> GetProductsByCategory(int? CategoryId , int? SubCategoryId);
        public IQueryable<Product> GetProductsByTag(int? Tager);





    }
}
