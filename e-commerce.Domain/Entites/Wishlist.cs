using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Wishlist")]
public partial class Wishlist
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [ForeignKey("CustomerId")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("WishlistId")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}