using Microsoft.Identity.Client;

namespace e_commerce.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public decimal? Discount { get; set; }
        public int? Stock { get; set; }
        public bool IsApproved { get; set; }
        public string Tag { get; set; } // mapped from enum
        public string SubCategoryName { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageViewModel> Images { get; set; }
        public List<ReviewViewMode> Reviews { get; set; }
        

    }
    public class ProductImageViewModel
    {
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }

   

}
