using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Order")]
public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int CustomerId { get; set; }

    [Column("Order_Date", TypeName = "datetime")]
    public DateTime? OrderDate { get; set; }

    public int Status { get; set; }

    [Column("Payment_Method")]
    public int PaymentMethod { get; set; }

    [Column("Total_Price", TypeName = "decimal(10, 2)")]
    public decimal TotalPrice { get; set; }

    [Column("Shipping_Address_ID")]
    public int ShippingAddressId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    [InverseProperty("Order")]
    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();

    [ForeignKey("ShippingAddressId")]
    [InverseProperty("Orders")]
    public virtual Address ShippingAddress { get; set; }
}