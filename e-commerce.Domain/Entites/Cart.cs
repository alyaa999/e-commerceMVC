using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace e_commerce.Infrastructure.Entites;

[Table("Cart")]
[Index("CustomerId", Name = "UQ__Cart__8CB286B8700ED883", IsUnique = true)]
public partial class Cart
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Total_Price", TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [Column("Total_Items_Number")]
    public int TotalItemsNumber { get; set; }

    [Column("Customer_ID")]
    public int CustomerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedDate { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    [ForeignKey("CustomerId")]
    [InverseProperty("Cart")]
    public virtual Customer Customer { get; set; }
}