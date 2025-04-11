using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Product_Image")]
public partial class ProductImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Product_ID")]
    public int ProductId { get; set; }

    [Required]
    [Column("Image_URL")]
    [StringLength(255)]
    public string ImageUrl { get; set; }

    [Column("Is_Primary")]
    public bool? IsPrimary { get; set; }

    [Column("Display_Order")]
    public int? DisplayOrder { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("ProductImages")]
    public virtual Product Product { get; set; }
}