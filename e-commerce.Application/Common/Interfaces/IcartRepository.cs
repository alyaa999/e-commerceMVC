using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IcartRepository
    {

     int GetCartItemsByCustomerID(int customerId);

            Cart? GetCartByCustomerId(int customerId);
        void AddItemToCart(int cartId, int productId, int quantity);
        public void UpdateItemQuantity(int userId, int productId, int newQuantity);
        void RemoveAllFromCart(int cartId,int custid);
        void SaveChanges();
        public void RemoveItemFromCart(int userId, int productId);

    }
}
