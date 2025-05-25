using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Application.Common.Interfaces
{
    public interface IOrderAdminRepo
    {
        Task<Order> GetByIdAsync(int id);
        Task<Order> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Order>> GetAllWithDetailsAsync();
        Task ConfirmOrderAsync(int id);
        Task CancelOrderAsync(int id, string reason = null);
        Task UpdateAsync(Order order);

        Task<Order?> GetOrderWithDetailsAsync(int id);
    }
}
