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
    
    public class returnRepoService :IReturnRepository
    {
        private ECommerceDBContext context;
        private IOrderRepository OrderRepository;
        public returnRepoService(ECommerceDBContext _context, IOrderRepository orderRepository)
        {
            context = _context;
            OrderRepository = orderRepository;
        }

        public void AddReturnRequest(List<Return> returns)
        {
            if (returns != null) {
                for (int i = 0; i < returns.Count; i++) { 
                 context.Returns.Add(returns[i]);
                 var order = OrderRepository.getOrderByOrderID(returns[i].custId,returns[i].OrderId).ReturnStatus = Domain.Enums.ReturnStatusEnum.Pending;
                }
                context.SaveChanges();
            }
        }

        public List<Return> getAllCustomerReturns(int custID)
        {
            return context.Returns.Include(r=>r.Order).Include(r=>r.Product).Where(re=>re.custId==custID).ToList();
        }
        public List<Order> getOrdersCanReturn(int userId)
        {
            var orders = OrderRepository.viewAllOrders(userId);
            var returnOrders = new List<Order>();
            if (orders != null)
            {
                 returnOrders = orders
                           .Where(o => ( o.ReturnStatus != Domain.Enums.ReturnStatusEnum.Pending && o.ReturnStatus != Domain.Enums.ReturnStatusEnum.Rejected && o.ReturnStatus != Domain.Enums.ReturnStatusEnum.Approved) && 
                                       o.Status == Domain.Enums.orderstateEnum.Delivered && 
                                       o.PaymentStatus == Domain.Enums.PaymentStatusEnum.Paid &&
                                       o.OrderDate.HasValue &&
                                       (DateTime.Now - o.OrderDate.Value).TotalDays <= 14)
                           .ToList();
            }
            return returnOrders;
        }
        
    }
}
