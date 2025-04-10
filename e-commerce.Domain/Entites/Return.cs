﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

public partial class Return
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string Reason { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; }

    [Column("Order_ID")]
    public int OrderId { get; set; }

    [Column("Product_ID")]
    public int ProductId { get; set; }

    [Column("Return_Date", TypeName = "datetime")]
    public DateTime? ReturnDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Returns")]
    public virtual Order Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Returns")]
    public virtual Product Product { get; set; }

    [InverseProperty("Return")]
    public virtual ICollection<ReturnImage> ReturnImages { get; set; } = new List<ReturnImage>();
}