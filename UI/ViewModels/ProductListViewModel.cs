using e_commerce.Infrastructure.Entites;

namespace e_commerce.Web.ViewModels
{
    public class ProductListViewModel
    {
       
       
        public int? SubCategoryId { get; set; }
        public List<Product> Products { get; set; } = new();
        public List<SubCategory> SubCategories { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;

    }
}
