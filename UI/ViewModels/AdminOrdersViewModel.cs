using System;
using System.Collections.Generic;
using e_commerce.Infrastructure.Entites;


namespace e_commerce.Web.ViewModels
{
    public class AdminOrdersViewModel
    {
        public List<Order> Ordersbb { get; set; }
        public int? StatusFilter { get; set; }
        public string CustomerSearch { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
