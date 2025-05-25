using e_commerce.Infrastructure.Entites;
using Microsoft.Identity.Client;

namespace e_commerce.Web.ViewModels.Home
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public int? TagId { get; set; } 
        public decimal? Discount { get; set; }
        public int? Stock { get; set; }
        public bool IsApproved { get; set; }
        public int? SellerId { get; set; }
        public string? Tag { get; set; } // mapped from enum
        public string? SubCategoryName { get; set; }
        public int? SubCategoryId { get; set; }  
        public string? CategoryName { get; set; }
        public int? CategoryId { get; set; }    

        public List<ProductImageViewModel>? Images { get; set; }
        public List<ReviewViewMode>? Reviews { get; set; }
        public List<IFormFile>? ImagesUpload { get; set; }
        public int PrimaryImageIndex { get; set; }



    }
    public class ProductImageViewModel
    {
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }

   

}
