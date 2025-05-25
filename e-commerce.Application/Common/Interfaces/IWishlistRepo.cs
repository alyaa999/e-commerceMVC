using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerce.Infrastructure.Entites;
namespace e_commerce.Application.Common.Interfaces
{
    public interface IWishlistRepo
    {
         int GetWishListCount(int customerId);

        Task AddAsync(Wishlist entity);
        Task<Wishlist> DeleteAsync(int id);
        Task<List<Wishlist>> GetAllAsync();
        Task<Wishlist> GetByIdAsync(int id);
        void Update(Wishlist entity);
        Wishlist GetByCustomerId(int customerId);
        public Task<Boolean> addToWishlist(int pid);
        public Task<bool> removeFromWishlist(int pid);
    }
}
