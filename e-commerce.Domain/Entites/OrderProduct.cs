using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[PrimaryKey("OrderId", "ProductId")]
[Table("Order_Product")]
public partial class OrderProduct
{
    [Key]
    [Column("Order_ID")]
    public int OrderId { get; set; }

    [Key]
    [Column("Product_ID")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    [Column("Unit_Price", TypeName = "decimal(10, 2)")]
    public decimal UnitPrice { get; set; }

    [Column("Item_Total", TypeName = "decimal(10, 2)")]
    public decimal ItemTotal { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderProducts")]
    public virtual Order Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("OrderProducts")]
    public virtual Product Product { get; set; }
}