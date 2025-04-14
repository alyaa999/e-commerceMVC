using System.ComponentModel.DataAnnotations;

namespace e_commerce.Web.ViewModels
{
    public class AddressVM
    {
        public string City { get; set; }

        public string Street { get; set; }

        public string DeptNo { get; set; }

        public bool IsDefault { get; set; }
    }
}
