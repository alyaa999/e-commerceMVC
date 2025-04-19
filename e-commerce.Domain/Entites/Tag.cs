using e_commerce.Infrastructure.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.Entites
{
    [Table("Tager")]

    public class Tag
    {
        [Key]
        public int Id { get; set; } 
        public String Name { get; set; }
        public ICollection<Product> Products { get; set; }
        
    }
}
