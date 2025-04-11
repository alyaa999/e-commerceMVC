using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Review")]
public partial class Review
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Customer_ID")]
    public int CustomerId { get; set; }

    [Column("Product_ID")]
    public int ProductId { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }

    [Column("Review_Date", TypeName = "datetime")]
    public DateTime? ReviewDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Reviews")]
    public virtual Customer Customer { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Reviews")]
    public virtual Product Product { get; set; }
}