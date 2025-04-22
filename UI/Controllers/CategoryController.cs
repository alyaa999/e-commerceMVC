using e_commerce.Application.Common.Interfaces;
using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly IRepository<Category> _categoryRepo;

    public CategoryController(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    // Index Action - to display the list of categories
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryRepo.GetAllIncludingAsync(c => c.SubCategories);
        return View(categories);
    }

    // Get Create Action - to display the create form
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Post Create Action - to handle form submission for creating a category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (ModelState.IsValid)
        {
            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveChangesAsync(); // Save changes to the database
            TempData["Success"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // Get Edit Action - to display the edit form
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    // Post Edit Action - to handle form submission for editing a category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _categoryRepo.Update(category);
                await _categoryRepo.SaveChangesAsync(); // Ensure changes are saved to the database
                TempData["Success"] = "Category updated successfully!";
            }
            catch
            {
                TempData["Error"] = "Error updating category!";
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }

    // Post Delete Action - to delete the category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        try
        {
            await _categoryRepo.DeleteAsync(id);
            await _categoryRepo.SaveChangesAsync(); // Ensure changes are saved to the database
            TempData["Success"] = "Category deleted successfully!";
        }
        catch
        {
            TempData["Error"] = "Error deleting category! It may have associated subcategories.";
        }

        return RedirectToAction(nameof(Index));

    }
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var categories = await _categoryRepo.GetAllIncludingAsync(c => c.SubCategories);
        var category = categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

}
