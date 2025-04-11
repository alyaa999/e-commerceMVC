using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

[Table("Return_Image")]
public partial class ReturnImage
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("Return_ID")]
    public int ReturnId { get; set; }

    [Required]
    [Column("Image_URL")]
    [StringLength(255)]
    public string ImageUrl { get; set; }

    [Column("Is_Primary")]
    public bool? IsPrimary { get; set; }

    [Column("Display_Order")]
    public int? DisplayOrder { get; set; }

    [ForeignKey("ReturnId")]
    [InverseProperty("ReturnImages")]
    public virtual Return Return { get; set; }
}