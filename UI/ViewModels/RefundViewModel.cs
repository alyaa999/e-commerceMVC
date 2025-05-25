namespace e_commerce.Web.ViewModels
{
    public class RefundViewModel
    {
        public int OrderId { get; set; }
        public int ReturnId { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
