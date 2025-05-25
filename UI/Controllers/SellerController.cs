using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Entites;
using e_commerce.Domain.Enums;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly IRepository<Seller> _sellerRepo;

        private readonly IRepository<Product> _productRepo;
        public Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager { get; }
        public ECommerceDBContext _context { get; }

        public SellerController(IRepository<Seller> sellerRepo , IRepository<Product> productRepo, UserManager<ApplicationUser> userManager , ECommerceDBContext dbContext)
        {
            _sellerRepo = sellerRepo;
            _productRepo = productRepo;
            _userManager = userManager;
            _context = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();
            var products = await _productRepo.FindAsync(i => i.SellerId == seller.Id);
            
            var totalSold = await _context.Products
            .Where(p => p.SellerId == seller.Id)
            .SelectMany(p => p.OrderProducts)
            .SumAsync(op => (int?)op.Quantity) ?? 0;

            var totalRevenue = await _context.Products
             .Where(p => p.SellerId == seller.Id)
             .SelectMany(p => p.OrderProducts)
             .SumAsync(op => (decimal?)(op.Quantity * op.UnitPrice)) ?? 0;

            
            DateTime oneWeekAgo = DateTime.Now.AddDays(-7);

            var query = _context.OrderProducts
                .Where(op => op.Product.SellerId == seller.Id)
                .Select(op => op.Order)
                .Distinct(); // Avoid double-counting the same order if multiple products in it

            int PendingOrdersCount = query.Count(o => o.Status == orderstateEnum.Pending);
            int deliveredOrdersCount = query.Count(o => o.Status == orderstateEnum.Delivered );
            ViewBag.productsCount = products.Count();

            ViewBag.Revenue = totalRevenue;
            ViewBag.Sold = totalSold;
            ViewBag.PendingOrdersCount = PendingOrdersCount;
            ViewBag.DeliveredOrdersCount = deliveredOrdersCount;



            return View();
        }

        public async Task<IActionResult> GetSalesData()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Json(new { error = "User not found" });

                var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).FirstOrDefault();
                if (seller == null) return Json(new { error = "Seller not found" });

                // Get monthly sales data grouped by OrderDate's month
                var monthlySales = await _context.Orders
                     .Where(o => o.OrderDate != null) // Filter out NULL dates
                     .GroupBy(o => new {
                         Year = o.OrderDate.Value.Year,
                         Month = o.OrderDate.Value.Month
                     })
                     .Select(g => new {
                         Year = g.Key.Year,
                         Month = g.Key.Month,
                         MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key.Month),
                         TotalSales = g.Sum(o => o.TotalPrice),
                         OrderCount = g.Count()
                     })
                     .OrderBy(x => x.Year)
                     .ThenBy(x => x.Month)
                     .ToListAsync();

                // If you want data for the current year only:
                var currentYear = DateTime.Now.Year;
                var currentYearSales = monthlySales
                    .Where(x => x.Year == currentYear)
                    .ToList();

                // Fill in missing months with 0
                var result = Enumerable.Range(1, 12)
                    .Select(month => new {
                        Month = month,
                        MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month),
                        TotalSales = currentYearSales.FirstOrDefault(ms => ms.Month == month)?.TotalSales ?? 0
                    })
                    .ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

    }
}
