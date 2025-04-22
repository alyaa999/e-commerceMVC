using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using e_commerce.Infrastructure.Entites;
using e_commerce.Application.Common.Interfaces;
using System.Security.Claims;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class WishlistsController : Controller
    {
        private readonly ECommerceDBContext _context;
        private readonly IWishlistRepo repo;
        private ICustRepo custrepo;
        private readonly IHomeRepository homeRepository;
        private readonly IcartRepository CartRepository;
        private readonly IMapper _mapper;
        public WishlistsController(ECommerceDBContext context,IWishlistRepo repository, ICustRepo _custrepo,IHomeRepository home,IcartRepository icartRepository,IMapper mapper)
        {
            _context = context;
            repo = repository;
            custrepo = _custrepo;
            homeRepository = home;
            CartRepository = icartRepository;
            _mapper = mapper;

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            base.OnActionExecuting(filterContext);
            var DbCategories = homeRepository.GetCategories();
            var cartItemCount = CartRepository.GetCartByCustomerId(custrepo.getcustomerid(userId).Id).CartProducts?.Count ?? 0;
            ViewBag.CartItemCount = cartItemCount;
            var WishlistItemCount = repo.GetByCustomerId(custrepo.getcustomerid(userId).Id).Products?.Count ?? 0;
            ViewBag.WishlistItemCount = WishlistItemCount;
            var categories = _mapper.Map<List<CategoryViewModel>>(DbCategories?.ToList() ?? new List<Category>());
            ViewBag.Categories = categories;
        }
        // GET: Wishlists
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound();
            }
            else 
            {
                var wishlist =  repo.GetByCustomerId(custrepo.getcustomerid(userId).Id);
                return View(wishlist);
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            try
            {
               var res = await repo.addToWishlist(productId);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
