using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Address")]
public partial class Address
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(100)]
    public string Street { get; set; }

    [Required]
    [StringLength(100)]
    public string DeptNo { get; set; }

    public bool IsDefault { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Addresses")]
    public virtual Customer Customer { get; set; }

    [InverseProperty("ShippingAddress")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}