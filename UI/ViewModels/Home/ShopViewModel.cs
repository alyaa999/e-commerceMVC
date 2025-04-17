using e_commerce.Infrastructure.Entites;

namespace e_commerce.Web.ViewModels.Home
{
    public class ShopViewModel
    {
        public int?  productCount { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? TotalPages { get; set; }
        public int? PageNumber { get; set; }
        public List<string>  BrandFilter { get; set; }
        public int? PriceFilter { get; set; }
        public List<int?> TagFilter { get; set; }
        public string Name { get; set; }
        public List<string>? Brands { get; set; }
        public List<Tager> tagers { get; set; }
        public List<ProductViewModel>? Products { get; set; }
        
    }
}
