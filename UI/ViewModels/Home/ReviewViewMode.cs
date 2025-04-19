using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Web.ViewModels.Home
{
    public class ReviewViewMode
    {
        
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = "Customer Name";


        public int ProductId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime? ReviewDate { get; set; }



    }
}
