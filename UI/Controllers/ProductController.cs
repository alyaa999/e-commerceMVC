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
    [Authorize(Roles = "Seller")]
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

                if (productView.ImagesUpload != null && productView.ImagesUpload.Count > 0)
                {
                    for (int i = 0; i < productView.ImagesUpload.Count; i++)
                    {
                        var image = productView.ImagesUpload[i];
                        if (image.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            var productImage = new ProductImage
                            {
                                ImageUrl = fileName,
                                IsPrimary = productView.PrimaryImageIndex == i
                            };

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
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();


            var products = _productRepo.Find(i=>i.SellerId == seller.Id, x => x.ProductImages);
            var productViewModels = Mapper.Map<List<ProductViewModel>>(products);   
            return View(productViewModels);
        }
        public IActionResult NotFoundPage()
        {
            return View("NotFound");
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();
            var products = await  _productRepo.FindAsync(i=>i.SellerId ==seller.Id && i.Id==id  , x=>x.ProductImages);
            var product = products.FirstOrDefault();

            if (product == null)
            {
                return View("NotFound");
            }

            var productViewModel = Mapper.Map<ProductViewModel>(product);
            return View(productViewModel);
        }

        // GET: product/edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
            var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();
            var products = await _productRepo.FindAsync(i => i.SellerId == seller.Id && i.Id == id, x => x.ProductImages);
            var product = products.FirstOrDefault();
            if (product == null)
            {
                return View("NotFound");
            }
            var productViewModel = Mapper.Map<ProductViewModel>(product);

            return View(productViewModel);
        }

        // POST: product/edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productView)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User); // Get the currently logged-in user
                var seller = _sellerRepo.Find(i => i.ApplicationUserId == user.Id).First();
                // Fetch the product from the database
                var productObj = _productRepo.Find(i=>i.Id == productView.Id,x=>x.ProductImages).FirstOrDefault();
                
                if (productObj == null)
                {
                    return NotFound();
                }
               
                // Use AutoMapper to map the basic fields of the productView to productObj
                Mapper.Map(productView, productObj);
                productObj.SellerId = seller.Id;



                // If new images are uploaded
                if (productView.ImagesUpload != null && productView.ImagesUpload.Count > 0)
                {
                    // Optional: Delete old images if you want to replace them (this could be adjusted based on your needs)
                    foreach (var img in productObj.ProductImages)
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", img.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Clear existing images from the product (if you're replacing all images)
                    productObj.ProductImages.Clear();

                    // Process the new uploaded images
                    for (int i = 0; i < productView.ImagesUpload.Count; i++)
                    {
                        var image = productView.ImagesUpload[i];
                        if (image.Length > 0)
                        {
                            // Generate a unique file name
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image", fileName);

                            // Save the image to the file system
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            // Create ProductImage object and assign IsPrimary based on the provided index
                            var productImage = new ProductImage
                            {
                                ImageUrl = fileName,
                                IsPrimary = (productView.PrimaryImageIndex == i)
                            };

                            productObj.ProductImages.Add(productImage);
                        }
                    }
                }
                else
                {
                    // No new images uploaded, so update the primary image
                    // Unset the primary image for all images
                    foreach (var img in productObj.ProductImages)
                    {
                        img.IsPrimary = false;
                    }

                    // Set the primary image based on the selected index (if valid)
                    if (productView.PrimaryImageIndex >= 0 && productView.PrimaryImageIndex < productObj.ProductImages.Count)
                    {
                        //productObj.ProductImages[productView.PrimaryImageIndex].IsPrimary = true;
                    }
                }

                // Save the updated product to the database
                _productRepo.Update(productObj);
                await _productRepo.SaveChangesAsync();

                // Redirect to the Index page after saving
                return RedirectToAction(nameof(Index));
            }

            // In case of validation errors, re-populate dropdowns or other fields
            return View(productView);
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
                _productRepo.SaveChanges(); // Save changes to the database
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
