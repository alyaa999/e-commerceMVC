//using Microsoft.AspNetCore.Mvc;



//    using Microsoft.AspNetCore.Authorization;
//    using Microsoft.AspNetCore.Identity;
//    using Microsoft.AspNetCore.Mvc;
//    using System.Threading.Tasks;
//    using global::e_commerce.Application.Common.Interfaces;
//    using global::e_commerce.Domain.Entites;
//    using global::e_commerce.Infrastructure.Entites;

//    namespace e_commerce.Web.Controllers
//    {
//        [Authorize]
//        public class ProductController : Controller
//        {
//            private readonly IRepository<Product> _productRepo;
//            private readonly IRepository<SubCategory> _subCategoryRepo;
//            private readonly IRepository<Order> _orderRepo;
//            private readonly IRepository<Customer> _customerRepo;
//            private readonly UserManager<ApplicationUser> _userManager;

//            // Constructor accepting the repositories and UserManager
//            public ProductController(
//                IRepository<Product> productRepo,
//                IRepository<SubCategory> subCategoryRepo,
//                IRepository<Order> orderRepo,
//                IRepository<Customer> customerRepo,
//                UserManager<ApplicationUser> userManager)
//            {
//                _productRepo = productRepo;
//                _subCategoryRepo = subCategoryRepo;
//                _orderRepo = orderRepo;
//                _customerRepo = customerRepo;
//                _userManager = userManager;
//            }

//            // GET: Product/Create
//            public IActionResult Create()
//            {
//                return View();
//            }

//            // POST: Product/Create
//            [HttpPost]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> Create(Product product)
//            {
//                if (ModelState.IsValid)
//                {
//                    var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
//                    product.ApplicationUserId = user.Id; // Set the logged-in user's ID to the product

//                    // Add the product using the repository
//                    await _productRepo.AddAsync(product);

//                    TempData["Success"] = "Product added successfully!";
//                    return RedirectToAction(nameof(Index)); // Redirect to product listing page or another page
//                }

//                return View(product);
//            }

//            // You can add other actions for listing, editing, etc., like this:

//            // GET: Product/Index
//            public async Task<IActionResult> Index()
//            {
//                var products = await _productRepo.GetAllAsync();
//                return View(products);
//            }

//            // GET: Product/Edit/{id}
//            public async Task<IActionResult> Edit(int id)
//            {
//                var product = await _productRepo.GetByIdAsync(id);
//                if (product == null)
//                {
//                    return NotFound();
//                }

//                return View(product);
//            }

//            // POST: Product/Edit/{id}
//            [HttpPost]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> Edit(int id, Product product)
//            {
//                if (id != product.Id)
//                {
//                    return NotFound();
//                }

//                if (ModelState.IsValid)
//                {
//                    try
//                    {
//                        _productRepo.Update(product); // Update the product using the repository
//                        TempData["Success"] = "Product updated successfully!";
//                    }
//                    catch
//                    {
//                        TempData["Error"] = "Error updating the product!";
//                    }
//                    return RedirectToAction(nameof(Index)); // Redirect to the product listing
//                }

//                return View(product);
//            }

//            // POST: Product/Delete/{id}
//            [HttpPost]
//            [ValidateAntiForgeryToken]
//            public async Task<IActionResult> Delete(int id)
//            {
//                var product = await _productRepo.GetByIdAsync(id);
//                if (product == null)
//                {
//                    return NotFound();
//                }

//                try
//                {
//                    await _productRepo.DeleteAsync(id); // Delete the product using the repository
//                    TempData["Success"] = "Product deleted successfully!";
//                }
//                catch
//                {
//                    TempData["Error"] = "Error deleting the product!";
//                }

//                return RedirectToAction(nameof(Index)); // Redirect to product listing
//            }
//        }
//    }
using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
//using e_commerce.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly IRepository<Product> _productRepo;

    // Dependency injection for the repository
    public ProductController(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    // This action will insert a test product
    public async Task<IActionResult> AddTestProduct()
    {
        // Creating a new product object
        var product = new Product
        {
            Name = "Test Product",
            Price = 19.99M,
            SubCategoryId = 1 // Use a valid SubCategoryId from your database
        };

        // Adding the product to the database using the repository
        await _productRepo.AddAsync(product);

        return Content("Test product added successfully!");
    }
}
