using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.ViewModels;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    [ServiceFilter(typeof(LayoutDataFilterAttribute))]

    public class CartController : Controller
    {
        private IcartRepository cartreposervice;
        private ICustRepo custrepo;
        private readonly IHomeRepository homeRepository;
        private readonly IMapper _mapper;
        private readonly IWishlistRepo wishlistRepo;
        public CartController(IcartRepository _cartreposervice, ICustRepo _custrepo,IHomeRepository homeRepository,IMapper mapper,IWishlistRepo wishlist)
        {
            cartreposervice = _cartreposervice;
            custrepo = _custrepo;
            this.homeRepository = homeRepository;
            _mapper = mapper;
            wishlistRepo = wishlist;
        }
      
        // GET: CartController
        public ActionResult viewcartproducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = cartreposervice.GetCartByCustomerId(custrepo.getcustomerid(userId).Id);
            return View(cart);
        }

        // GET: CartController/Details/5
        

        // GET: CartController/Create
        public ActionResult Create()
        { 

            return View();
        }

        // POST: CartController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AddItemToCart(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Cart cart = cartreposervice.GetCartByCustomerId(custrepo.getcustomerid(userId).Id);
            cartreposervice.AddItemToCart(cart.Id,productId,1);
            return Json(new
            {
                success = true,
            });
        }

        // GET: CartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CartController/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UpdateCart([FromBody] List<CartUpdateDto> updates)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var item in updates)
                {
                    cartreposervice.UpdateItemQuantity(
                        custrepo.getcustomerid(userId).Id,
                        item.ProductId,
                        item.Quantity
                    );
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: CartController/Delete/5
        [HttpPost]
        public ActionResult DeleteItemFromCart(int PrdId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (PrdId != null)
                {
                    cartreposervice.RemoveItemFromCart(custrepo.getcustomerid(userId).Id, PrdId);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }

            return Json(new { success = true });
        }
       
    }
}
