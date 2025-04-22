using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.DTOS;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.Controllers;
using e_commerce.Web.Models;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Controllers;

[AllowAnonymous]
[ServiceFilter(typeof(LayoutDataFilterAttribute))]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHomeRepository homeRepository;
    private readonly IMapper _mapper;
    private readonly IcartRepository CartRepository;
    private readonly ICustRepo custRepo;
    private readonly IWishlistRepo wishlistRepo;

    
    public HomeController(ILogger<HomeController> logger , IHomeRepository homeRepository , IMapper mapper,IcartRepository repository,ICustRepo cust,IWishlistRepo wishlist)
    {
        _logger = logger;
        _mapper = mapper;   
        this.homeRepository = homeRepository;
        CartRepository = repository;
        custRepo = cust;
        wishlistRepo = wishlist;
    }
   

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        int pageSize = 2;

        var items = homeRepository.GetProductsByCategory(null, null);
        var paginatedList = await PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);

        var newArrivals = await PaginatedList<Product>.CreateAsync(homeRepository.GetProductsByTag(3), 1, 4);
        var trending = await PaginatedList<Product>.CreateAsync(homeRepository.GetProductsByTag(8), 1, 4);
        var hotRelease = await PaginatedList<Product>.CreateAsync(homeRepository.GetProductsByTag(5), 1, 4);
        var bestSeller = await PaginatedList<Product>.CreateAsync(homeRepository.GetProductsByTag(4), 1, 4);
        var deals = await PaginatedList<Product>.CreateAsync(homeRepository.GetProductsByTag(6), 1, 4);

        var homeViewModel = new HomeViewModel
        {
            Products = _mapper.Map<List<ProductViewModel>>(paginatedList),
            NewArrivalsProducts = _mapper.Map<List<ProductViewModel>>(newArrivals),
            Trending = _mapper.Map<List<ProductViewModel>>(trending),
            HotRelease = _mapper.Map<List<ProductViewModel>>(hotRelease),
            BestDeal = _mapper.Map<List<ProductViewModel>>(deals),
            TopSelling = _mapper.Map<List<ProductViewModel>>(bestSeller),
        };

        return View(homeViewModel);
    }

    //In AutoMapper, mapping a valid source object (like a list) will usually return an empty list, not null, even if the source list is empty or null.
    public async  Task<IActionResult> ShopNow(ShopViewModel ShopVm, int? Filter, int pageNumber= 1 )
    {
        int pageSize = 1;
        if(Filter != null)
        {
            ShopVm.TagFilter.Add(Filter.Value);
        }
        var shopDTO = _mapper.Map<ShopDTO>(ShopVm);
       
        var items = homeRepository.GetProducts(shopDTO);
        var paginatedList = await  PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);

        var brands = homeRepository.GetBrands();
        var tagersObj = homeRepository.GetTagers();
        var tagers = _mapper.Map<List<TagViewModel>>(tagersObj);


        var shopViewMode = new ShopViewModel()
        {
            CategoryId = ShopVm.CategoryId,
            SubCategoryId = ShopVm.SubCategoryId,
            BrandFilter = ShopVm.BrandFilter,
            PriceFilter = ShopVm.PriceFilter,
            TagFilter = ShopVm.TagFilter,
            PageNumber = pageNumber,
            productCount = items.Count(),
            TotalPages = paginatedList.TotalPages,
            Brands = brands,
            tagers = tagers,
            Name = ShopVm.Name,
            
            Products = _mapper.Map<List<ProductViewModel>>(paginatedList)
        };
      

        return View(shopViewMode);
    }
    
    public async Task<IActionResult> ProductDetials(int id)
    {
        var product = homeRepository.GetProductById(id);
        var relatedProduct =homeRepository.GetProductsByCategory(null, product?.SubCategoryId);
        var productDetails = new ProductDetialsViewModel
        {
            ProductDetials = _mapper.Map<ProductViewModel>(product),
            RelatedProducts= _mapper.Map<List<ProductViewModel>>(relatedProduct.ToList())
        };

        return View(productDetails);
    }

  

   
 
   
   

   
}
