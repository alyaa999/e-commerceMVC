using e_commerce.Infrastructure.Entites;

namespace e_commerce.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<Product> PendingProducts { get; set; }
        public List<Order> RecentOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSubCategories { get; set; }
        public int TotalCustomers { get; set; }
    }
}
