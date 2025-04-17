using System.Diagnostics;
using AutoMapper;
using e_commerce.Domain.DTOS;
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
            Products = _mapper.Map<List<ProductViewModel>>(paginatedList?.ToList()),
           NewArrivalsProducts = _mapper.Map<List<ProductViewModel>>(newArrivals?.ToList()),
            Trending = _mapper.Map<List<ProductViewModel>>(trending?.ToList()),
            HotRelease = _mapper.Map<List<ProductViewModel>>(hotRelease?.ToList()),
            BestDeal = _mapper.Map<List<ProductViewModel>>(Deals?.ToList() ),
            TopSelling = _mapper.Map<List<ProductViewModel>>(BestSeller?.ToList()),

        };






        return View(homeViewModel);
    }
    //In AutoMapper, mapping a valid source object (like a list) will usually return an empty list, not null, even if the source list is empty or null.
    public async  Task<IActionResult> ShopNow(ShopViewModel ShopVm,int pageNumber= 1)
    {
        int pageSize = 1;
        var shopDTO = _mapper.Map<ShopDTO>(ShopVm);
        var items = homeRepository.GetProducts(shopDTO);
        var brands = homeRepository.GetBrands();
        var tagers = homeRepository.GetTagers();
        var paginatedList = await PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);


        var products = paginatedList.ToList();
        var shopViewMode = new ShopViewModel()
        {
            CategoryId = ShopVm.CategoryId,
            SubCategoryId = ShopVm.SubCategoryId,
            BrandFilter = ShopVm.BrandFilter,
            PriceFilter = ShopVm.PriceFilter,
            TagFilter = ShopVm.TagFilter,
            PageNumber = pageNumber,
            productCount = products.Count(),
            TotalPages = paginatedList.TotalPages,
            Brands = brands,
            tagers = tagers,
            Name = ShopVm.Name,
            Products = _mapper.Map<List<ProductViewModel>>(products)
        };
      

        return View(shopViewMode);
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

  

   
 
   
   

   
}
