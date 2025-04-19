using System.ComponentModel.DataAnnotations;
namespace e_commerce.Web.Models
{
    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "New email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string NewEmail { get; set; }
    }
}
