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
    public class CarRepoService : IcartRepository
    {
        private ECommerceDBContext context;

        public CarRepoService(ECommerceDBContext _context)
        {
            context= _context;
        }

        public void AddItemToCart(int cartId, int productId, int quantity)
        {
            var existingItem = context.CartProducts
            .FirstOrDefault(cp => cp.CartId == cartId && cp.ProductCode == productId);

            if (existingItem != null)
            {
                // Update quantity if already exists by one
                existingItem.Quantity += quantity;
                existingItem.ItemTotal = existingItem.Quantity * existingItem.UnitPrice;
            }
            else
            {
                // Add new item
                var product = context.Products.Find(productId);
                if (product != null)
                {
                    context.CartProducts.Add(new CartProduct
                    {
                        CartId = cartId,
                        ProductCode = productId,
                        Quantity = quantity,
                        UnitPrice = product.Price,
                        ItemTotal = product.Price * quantity
                    });
                }
            }

            // Update cart totals
            var cart = context.Carts.Find(cartId);
            if (cart != null)
            {
                cart.TotalItemsNumber = context.CartProducts
                    .Where(cp => cp.CartId == cartId)
                    .Sum(cp => cp.Quantity);

                cart.TotalPrice = context.CartProducts
                    .Where(cp => cp.CartId == cartId)
                    .Sum(cp => cp.ItemTotal);
            }

            SaveChanges();
        }
        

        public Cart GetCartByCustomerId(int customerId)
        {
            Cart cart= context.Carts
            .Include(c => c.CartProducts)
            .ThenInclude(cp => cp.ProductCodeNavigation).ThenInclude(p=>p.ProductImages)
            .FirstOrDefault(c => c.CustomerId == customerId);
            if (cart == null) 
            {
                // Create new cart if one doesn't exist
                return new Cart();
            }
            else
            {
                return cart;
            }
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void UpdateItemQuantity(int userId, int productId, int newQuantity)
        {
            var cart = context.Carts
        .Include(c => c.CartProducts)
        .FirstOrDefault(c => c.CustomerId == userId);

            var item = cart?.CartProducts.FirstOrDefault(cp => cp.ProductCode == productId);

            if (item != null)
            {
                item.Quantity = newQuantity;
                item.ItemTotal = item.UnitPrice * newQuantity;

                // Update cart totals
                cart.TotalItemsNumber = cart.CartProducts.Sum(cp => cp.Quantity);
                cart.TotalPrice = cart.CartProducts.Sum(cp => cp.ItemTotal);

                context.SaveChanges();
            }
        }
    }
}
