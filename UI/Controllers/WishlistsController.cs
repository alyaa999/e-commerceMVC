using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce.Infrastructure.Entites;
using e_commerce.Application.Common.Interfaces;

namespace e_commerce.Web.Controllers
{
    public class WishlistsController : Controller
    {
        private readonly ECommerceDBContext _context;
        private readonly IWishlistRepo repo;
        public WishlistsController(ECommerceDBContext context,IWishlistRepo repository)
        {
            _context = context;
            repo = repository;
        }

        // GET: Wishlists
        public async Task<IActionResult> Index(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var wishlist = await repo.GetByCustomerId(id);
                return View(wishlist);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var res = await repo.addToWishlist(productId);
            return Json(new
            {
                success = true,
            });
        }

        // GET: Wishlists/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id != null)
                {
                    var ws = await repo.removeFromWishlist((int)id);
                }
            }
            catch(Exception ex)
            {
                    return Json(new { success = false, message = ex.Message });

            }

            return Json(new { success = true });
        }
    }
}
