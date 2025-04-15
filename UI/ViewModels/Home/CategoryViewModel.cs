using System.ComponentModel.DataAnnotations;

namespace e_commerce.Web.ViewModels.Home
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public List<SubCategoryViewModel> subCategory { get; set; } 
    }
}
