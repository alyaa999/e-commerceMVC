using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        public void AddOrder(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            var existingOrder = context.Orders.Find(order.Id);
            if (existingOrder != null)
            {
                context.Orders.Update(order);
                context.SaveChanges();
            }
        }
        public Order GetOrderById(int orderId)
        {
            return context.Orders
            .Include(o => o.OrderProducts)
            .FirstOrDefault(o => o.Id == orderId);
        }
        public void DeleteOrder(int orderId)
        {
            var order = context.Orders.Find(orderId);
            if (order != null)
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }
        }
        }

        public Order viewCustOrder(int userId,int orderID)
        {
            return context.Orders.Include(o => o.OrderProducts).ThenInclude(op=>op.Product).ThenInclude(p=>p.ProductImages).FirstOrDefault(o => ((o.Id == orderID) && (o.CustomerId == userId)));
        }
    }
}
