using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Seller")]
public partial class Seller
{
    [Key]
    [Column("ID")]

    public int Id { get; set; }

    public int? SellerCode { get; set; }


    [InverseProperty("Seller")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}