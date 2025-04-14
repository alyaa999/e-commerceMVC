using System.Diagnostics;
using AutoMapper;
using e_commerce.Infrastructure.Entites;
using e_commerce.Infrastructure.Repository;
using e_commerce.Web.Models;
using e_commerce.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        int pageSize = 1;
        var items = homeRepository.GetProductsByCategory(null, null);
    
        var paginatedList = await PaginatedList<Product>.CreateAsync(items, pageNumber, pageSize);

        var products= paginatedList.ToList();
        var productViewModels = _mapper.Map<List<ProductViewModel>>(products);
       
        return View(productViewModels);
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
        var productViewModel = _mapper.Map<ProductViewModel>(product);  
        return View(productViewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
