using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<SubCategory> _subCategoryRepo;
    private readonly IRepository<Order> _orderRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly IRepository<Seller> _sellerRepo;

    public AdminController(
        IRepository<Product> productRepo,
        IRepository<SubCategory> subCategoryRepo,
        IRepository<Order> orderRepo,
        IRepository<Seller> sellerRepo,
        IRepository<Customer> customerRepo)
    {
        _productRepo = productRepo;
        _subCategoryRepo = subCategoryRepo;
        _orderRepo = orderRepo;
        _customerRepo = customerRepo;
        _sellerRepo = sellerRepo;
    }

    public async Task<IActionResult> Index()
    {
      
        var allProducts = await _productRepo.GetAllAsync();
        var waitingProducts = allProducts.Where(p => !p.IsApproved).ToList();
        return View(waitingProducts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveProduct(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.IsApproved = true;
        _productRepo.Update(product);
        await _productRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApproveAllProducts()
    {
        var allProducts = await _productRepo.GetAllAsync();
        var pendingProducts = allProducts.Where(p => !p.IsApproved).ToList();

        foreach (var product in pendingProducts)
        {
            product.IsApproved = true;
            _productRepo.Update(product);
        }

        await _productRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectProduct(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        await _productRepo.DeleteAsync(id);
        await _productRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectAllProducts()
    {
        var allProducts = await _productRepo.GetAllAsync();
        var pendingProducts = allProducts.Where(p => !p.IsApproved).ToList();

        foreach (var product in pendingProducts)
        {
            await _productRepo.DeleteAsync(product.Id);
        }

        await _productRepo.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> GetSalesData()
    {
        var allOrders = await _orderRepo.GetAllAsync();
        var currentYear = DateTime.Now.Year;

        var salesData = allOrders
            .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Year == currentYear)
            .GroupBy(o => o.OrderDate.Value.Month)
            .Select(g => new {
                Month = g.Key,
                TotalSales = g.Sum(o => o.TotalPrice),
                OrderCount = g.Count()
            })
            .OrderBy(g => g.Month)
            .ToList();

        return Json(salesData);
    }

    public async Task<IActionResult> Chart()
    {
        var allOrders = await _orderRepo.GetAllAsync();
        ViewBag.OrdersNum = allOrders.Count();

        var allProducts = await _productRepo.GetAllAsync();
        ViewBag.ProductsNum = allProducts.Count();

        var allSubCategories = await _subCategoryRepo.GetAllAsync();
        ViewBag.SubCategoriesNum = allSubCategories.Count();

        var allCustomers = await _customerRepo.GetAllAsync();
        ViewBag.CustomersNum = allCustomers.Count();

        return View();
    }

    public async Task<IActionResult> Sellers(string searchString)
    {
        var allSellers = await _sellerRepo.GetAllIncludingAsync(s => s.ApplicationUser);

        if (!string.IsNullOrEmpty(searchString))
        {
            allSellers = allSellers.Where(s =>
                s.Id.ToString().Contains(searchString) ||
                (s.ApplicationUser.FirstName != null && s.ApplicationUser.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                (s.ApplicationUser.Email != null && s.ApplicationUser.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return View(allSellers);
    }

    public async Task<IActionResult> Products(int? sellerId, string searchString)
    {
        var allProducts = await _productRepo.GetAllAsync();
        var productsQuery = allProducts.AsQueryable();

        if (sellerId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.SellerId == sellerId.Value);
        }

        if (!string.IsNullOrEmpty(searchString))
        {
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                (p.Desc != null && p.Desc.Contains(searchString, StringComparison.OrdinalIgnoreCase)));
        }

        // Manually load seller information since we can't use GetAllIncludingAsync for products
        var productsWithSellers = new List<Product>();
        foreach (var product in productsQuery.ToList())
        {
            product.Seller = await _sellerRepo.GetByIdAsync((int)product.SellerId);
            productsWithSellers.Add(product);
        }

        ViewBag.Sellers = await _sellerRepo.GetAllAsync();
        return View(productsWithSellers);
    }



}