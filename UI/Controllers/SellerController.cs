using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Web.Controllers
{
    [Authorize(Roles = "Seller")]
    public class SellerController : Controller
    {
        private readonly IRepository<Seller> _sellerRepo;

        private readonly IRepository<Product> _productRepo;
        public SellerController(IRepository<Seller> sellerRepo , IRepository<Product> productRepo)
        {
            _sellerRepo = sellerRepo;
            _productRepo = productRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var seller = await _sellerRepo.GetByIdAsync(id);
                if (seller == null)
                {
                    TempData["Error"] = "Seller not found";
                    return RedirectToAction("Index");
                }

                return View(seller);
            }
            catch (Exception ex)
            {
           
                TempData["Error"] = "Error loading seller details";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Seller seller)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(seller);
                }

                var existingSeller = await _sellerRepo.GetByIdAsync(seller.Id);
                if (existingSeller == null)
                {
                    TempData["Error"] = "Seller not found";
                    return RedirectToAction("Index");
                }

      
                existingSeller.ApplicationUser.FirstName = seller.ApplicationUser.FirstName;
                existingSeller.ApplicationUser.LastName = seller.ApplicationUser.LastName;
                existingSeller.ApplicationUser.Email = seller.ApplicationUser.Email;
               
                _sellerRepo.Update(existingSeller);
                await _sellerRepo.SaveChangesAsync();

                TempData["Success"] = "Seller updated successfully";
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
             
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
                return View(seller);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An unexpected error occurred";
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var seller = await _sellerRepo.GetByIdAsync(id);
            if (seller == null)
                return NotFound();

            return View(seller);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid)
            {
                return View(seller);
            }

            await _sellerRepo.AddAsync(seller);
            return RedirectToAction("Index");
        }
    }
}
