using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    public class WishlistRepo : IWishlistRepo
    {
        ECommerceDBContext _context;
        public WishlistRepo(ECommerceDBContext context)
        {
            _context = context;
        }
        public Task AddAsync(Wishlist entity)
        {
            _context.Wishlists.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<Wishlist> DeleteAsync(int id)
        {

            var deleatedWishlist = _context.Wishlists.Find(id);
            if (deleatedWishlist != null)
            {
                _context.Wishlists.Remove(deleatedWishlist);
                _context.SaveChangesAsync();
                return Task.FromResult(deleatedWishlist);
            }
            else
            {
                throw new Exception("Wishlist not found");
            }
        }

        public Task<List<Wishlist>> GetAllAsync()
        {
            var wishlists = _context.Wishlists.Include(p => p.Products).ToList();
            return Task.FromResult(wishlists.ToList());
        }

        public Task<Wishlist> GetByIdAsync(int id)
        {
            var wishlist = _context.Wishlists.Find(id);
            if (wishlist != null)
            {
                return Task.FromResult(wishlist);
            }
            else
            {
                throw new Exception("Wishlist not found");
            }
        }

        public void Update(Wishlist entity)
        {
            var wishlist = _context.Wishlists.Find(entity.Id);
            if (wishlist != null)
            {
                _context.Wishlists.Update(entity);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Wishlist not found");
            }
        }
        public int GetWishListCount(int customerId)
        {
            var wishlist = _context.Wishlists.Include(p => p.Products).FirstOrDefault(w => w.CustomerId == customerId);
            if (wishlist != null)
            {
                return wishlist.Products.Count;
            }
            else
            {
                return 0;
            }

        }
        public Wishlist GetByCustomerId(int customerId)
        {
            var wishlist = _context.Wishlists.Include(p => p.Products).ThenInclude(i => i.ProductImages).FirstOrDefault(w => w.CustomerId == customerId);
            if (wishlist != null)
            {
                return wishlist;
            }
            else
            {
                return new Wishlist
                {
                    CustomerId = customerId,
                    Products = new List<Product>()
                };
            }
        }
        public async Task<bool> addToWishlist(int pid)
        {
            var product = await _context.Products.FindAsync(pid);
            if (product == null)
                throw new Exception("Product not found");

            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync();

            if (wishlist == null)
                throw new Exception("Wishlist not found");

            var alreadyExists = wishlist.Products.Any(p => p.Id == pid);
            if (alreadyExists)
                throw new Exception("Item already exists in wishlist");

            wishlist.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> removeFromWishlist(int pid)
        {
            var product = await _context.Products.FindAsync(pid);
            if (product == null)
                throw new Exception("Product not found");
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync();
            if (wishlist == null)
                throw new Exception("Wishlist not found");
            var alreadyExists = wishlist.Products.Any(p => p.Id == pid);
            if (!alreadyExists)
                throw new Exception("Item does not exist in wishlist");
            wishlist.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}

