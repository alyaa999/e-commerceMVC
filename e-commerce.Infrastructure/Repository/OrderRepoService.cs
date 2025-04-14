using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Infrastructure.Repository
{
    public class OrderRepoService : IOrderRepository
    {
        private ECommerceDBContext context;

        public OrderRepoService(ECommerceDBContext _context)
        {
            context = _context;
        }

        public List<Order> viewAllOrders(int userId)
        {
            return context.Orders
        .Where(o => o.CustomerId == userId)
        .ToList();
        }
        public void RemoveOrder(int cartId, int productId)
        {
            throw new NotImplementedException();
        }

    }
}
