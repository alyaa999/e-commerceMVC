using e_commerce.Domain.Entites;
using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.DTOS
{
    public class ShopDTO
    {
        public int? productCount { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? TotalPages { get; set; }
        public int? PageNumber { get; set; }
        public int? PriceFilter { get; set; }
        public string Name { get; set; }
        public List<string> BrandFilter { get; set; }
        public List<int?> TagFilter { get; set; }

    }
}
