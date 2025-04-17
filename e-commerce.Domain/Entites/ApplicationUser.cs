using e_commerce.Infrastructure.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.Domain.Entites
{
    public class ApplicationUser : IdentityUser 
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public Customer? Customer { get; set; }
        public Seller? Seller { get; set; }
    }

}
