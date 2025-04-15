using System.Diagnostics;
using AutoMapper;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.Models;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeRepository homeRepository;
    private readonly IMapper _mapper;
   
    public HomeController(ILogger<HomeController> logger , IHomeRepository homeRepository , IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;   
        this.homeRepository = homeRepository;
    }
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {

        var DbCategories = homeRepository.GetCategories();

        var categories = _mapper.Map<List<CategoryViewModel>>(DbCategories?.ToList() ?? new List<Category>());
        ViewBag.Categories = categories;
        base.OnActionExecuting(filterContext);
    }


    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        int pageSize = 1;
        var items = homeRepository.GetProductsByCategory(null, null);
        var newArrivalsProducts = homeRepository.GetProductsByTag(3); //3 is new Arrival
        var TrendingProducts = homeRepository.GetProductsByTag(8); //8 is trending 
        var HotReleaseProducts = homeRepository.GetProductsByTag(5); //5 is HotReleaseProduct
        var BestSellerProducts = homeRepository.GetProductsByTag(4); //4 is BestSeller
        var DealsProducts = homeRepository.GetProductsByTag(6); //6 is Deals
 


        var paginatedList = await PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);
        var newArrivals = await PaginatedList<Product>.CreateAsync(newArrivalsProducts, 1, 4);
        var trending = await PaginatedList<Product>.CreateAsync(TrendingProducts, 1, 4);
        var hotRelease = await PaginatedList<Product>.CreateAsync(HotReleaseProducts, 1, 4);
        var BestSeller = await PaginatedList<Product>.CreateAsync(BestSellerProducts, 1, 4);
        var Deals = await PaginatedList<Product>.CreateAsync(DealsProducts, 1, 4);
        


        var homeViewModel = new HomeViewModel
        {
            Products = _mapper.Map<List<ProductViewModel>>(paginatedList?.ToList() ?? new List<Product>()),
           NewArrivalsProducts = _mapper.Map<List<ProductViewModel>>(newArrivals?.ToList() ?? new List<Product>()),
            Trending = _mapper.Map<List<ProductViewModel>>(trending?.ToList() ?? new List<Product>()),
            HotRelease = _mapper.Map<List<ProductViewModel>>(hotRelease?.ToList() ?? new List<Product>()),
            BestDeal = _mapper.Map<List<ProductViewModel>>(Deals?.ToList() ?? new List<Product>() ),
            TopSelling = _mapper.Map<List<ProductViewModel>>(BestSeller?.ToList() ?? new List<Product>()),

        };






        return View(homeViewModel);
    }
    public async  Task<IActionResult> ShopNow( int? CategoryId , int? SubCategoryId , int pageNumber = 1)
    {
        int pageSize = 1;
        var items = homeRepository.GetProductsByCategory(CategoryId, SubCategoryId);

        var paginatedList = await PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);

        var products = paginatedList.ToList();
        var productViewModels = _mapper.Map<List<ProductViewModel>>(products);
        ViewData["productCount"] = items.Count();
        ViewData["CategoryId"] = CategoryId;
        ViewData["SubCategoryId"] = SubCategoryId;
        ViewData["TotalPages"] = paginatedList.TotalPages; 
        ViewData["PageNumber"]=pageNumber;

        return View(productViewModels);
    }
    public async Task<IActionResult> ProductDetials(int id)
    {
        var product = homeRepository.GetProductById(id);
        var relatedProduct = homeRepository.GetProductsByCategory(null, product?.SubCategoryId);
        var productDetails = new ProductDetialsViewModel
        {
            ProductDetials = _mapper.Map<ProductViewModel>(product),
            RelatedProducts= _mapper.Map<List<ProductViewModel>>(relatedProduct)
        };

        return View(productDetails);
    }

    public async Task<IActionResult> SearchByName(string Name )
    {
        var items = homeRepository.GetProductsByName(Name);
        var paginatedList = await PaginatedList<Product>.CreateAsync(items, 1, 4);
        var products = paginatedList.ToList();
        var productViewModels = _mapper.Map<List<ProductViewModel>>(products);
        ViewData["productCount"] = items.Count();
        ViewData["TotalPages"] = paginatedList.TotalPages;
        ViewData["PageNumber"] = 1;
        return View("ShopNow", productViewModels);
    }

   
 
   
   

   
}
