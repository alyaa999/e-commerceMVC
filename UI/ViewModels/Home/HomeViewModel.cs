using e_commerce.Infrastructure.Entites;

namespace e_commerce.Web.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<ProductViewModel> Products { get; set; }
        public List<ProductViewModel> NewArrivalsProducts { get; set; }
        public List<ProductViewModel> HotRelease { get; set; }
        public List<ProductViewModel> BestDeal { get; set; }
        public List<ProductViewModel> TopSelling { get;set; }
        public List<ProductViewModel> Trending { get; set; } 

    }
}
