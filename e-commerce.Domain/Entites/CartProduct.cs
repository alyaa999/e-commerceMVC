using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[PrimaryKey("CartId", "ProductCode")]
[Table("Cart_Product")]
public partial class CartProduct 
{
    [Key]
    [Column("Cart_ID")]
    public int CartId { get; set; }

    [Key]
    [Column("product_code")]
    public int ProductCode { get; set; }

    public int Quantity { get; set; }

    [Column("Unit_Price", TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("Item_Total", TypeName = "decimal(10, 2)")]
    public decimal ItemTotal { get; set; }

    [ForeignKey("CartId")]
    [InverseProperty("CartProducts")]
    public virtual Cart Cart { get; set; }

    [ForeignKey("ProductCode")]
    [InverseProperty("CartProducts")]
    public virtual Product ProductCodeNavigation { get; set; }
}