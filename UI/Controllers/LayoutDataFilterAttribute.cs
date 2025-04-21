using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using e_commerce.Controllers;
namespace e_commerce.Web.Controllers
{
    public class LayoutDataFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository homeRepository;
        private readonly IMapper _mapper;
        private readonly IcartRepository CartRepository;
        private readonly ICustRepo custRepo;
        private readonly IWishlistRepo wishlistRepo;
        public LayoutDataFilterAttribute(ILogger<HomeController> logger, IHomeRepository homeRepository, IMapper mapper, IcartRepository repository, ICustRepo cust, IWishlistRepo wishlist)
        {
            _logger = logger;
            _mapper = mapper;
            this.homeRepository = homeRepository;
            CartRepository = repository;
            custRepo = cust;
            wishlistRepo = wishlist;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (Controller)filterContext.Controller;
            var userId = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);

            

            var customerId = custRepo.getcustomerid(userId)?.Id;
            var cartItemCount = CartRepository.GetCartItemsByCustomerID(customerId ?? 0);
            controller.ViewBag.CartItemCount = cartItemCount;

            var wishListCount = wishlistRepo.GetWishListCount(customerId ?? 0);
            controller.ViewBag.WishlistItemCount = wishListCount;

            var dbCategories = homeRepository.GetCategories();
            var categories = _mapper.Map<List<CategoryViewModel>>(dbCategories?.ToList() ?? new List<Category>());
            controller.ViewBag.Categories = categories;

            base.OnActionExecuting(filterContext);
        }
    }

}
