using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using e_commerce.Application.Common.Interfaces;
using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using AutoMapper;
using e_commerce.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc.Filters;

namespace e_commerce.Web.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<SubCategory> _subcategoryRepo;
        private readonly IRepository<Order> _orderRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public IMapper Mapper { get; }
        public IRepository<Category> CategoryRep { get; }
        public IRepository<Tag> Tag { get; }
        public IRepository<Seller> _sellerRepo { get; }

        // Constructor accepting the repositories and userManager
        public ProductController(
            IRepository<Product> productRepo,
            IRepository<SubCategory> subcategoryRepo,
            UserManager<ApplicationUser> userManager ,IMapper mapper , IRepository<Category> categoryRep , IRepository<Tag> Tag, IRepository<Seller> seller)
        {
            _productRepo = productRepo;
            _subcategoryRepo = subcategoryRepo;
            _userManager = userManager;
            Mapper = mapper;
            CategoryRep = categoryRep;
            this.Tag = Tag;
            _sellerRepo = seller;
        }

        // GET: product/create
        public IActionResult Create()
        {
            return View();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var category = CategoryRep.GetAllIncluding(i => i.SubCategories);
            var Categories = Mapper.Map<List<CategoryViewModel>>(category);
            var tagObj = Tag.GetAll();
            var Tags = Mapper.Map<List<TagViewModel>>(tagObj);
            ViewBag.Categories = Categories;
            ViewBag.Tags = Tags;
        }
        // POST: product/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productView)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
                var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();
                productView.SellerId = seller.Id;
                var productObj = Mapper.Map<Product>(productView);
                productObj.ProductImages = new List<ProductImage>();

                if (productView.ImagesUpload != null && productView?.ImagesUpload.Count > 0)
                {
                    foreach (var image in productView.ImagesUpload)
                    {
                        if (image.Length  > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var productImage = new ProductImage();
                            productImage.ImageUrl = fileName;
                            productImage.IsPrimary = true;  

                            // Save `fileName` to DB or a list
                            productObj.ProductImages.Add(productImage);
                        }
                    }
                }

                // Add the product using the repository
                await _productRepo.AddAsync(productObj);
                _productRepo.SaveChanges();
                


                return RedirectToAction(nameof(Index)); // Redirect to product listing page or another page
            }

            return View(productView);
        }

        // You can add other actions for listing, editing, etc., like this:

        // GET: product/index
        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }

        // GET: product/edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: product/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepo.Update(product); // Update the product using the repository
                    TempData["Success"] = "Product updated successfully!";
                }
                catch
                {
                    TempData["Error"] = "Error updating the product!";
                }
                return RedirectToAction(nameof(Index)); // Redirect to the product listing
            }

            return View(product);
        }

        // POST: product/delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                await _productRepo.DeleteAsync(id); // Delete the product using the repository
                TempData["Success"] = "Product deleted successfully!";
            }
            catch
            {
                TempData["Error"] = "Error deleting the product!";
            }

            return RedirectToAction(nameof(Index)); // Redirect to product listing
        }
    }
}
