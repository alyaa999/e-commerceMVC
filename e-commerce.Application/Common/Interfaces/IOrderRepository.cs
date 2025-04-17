using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IOrderRepository
    {
        public List<Order> viewAllOrders(int userId);
        public Order viewCustOrder(int userId, int orderID);

        public void RemoveOrder(int cartId, int productId);
        public void AddOrder(Order order);

    }
}
