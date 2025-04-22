using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_commerce.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using IndexAttribute = Microsoft.EntityFrameworkCore.IndexAttribute;

namespace e_commerce.Infrastructure.Entites;

[Table("Product")]
[Index("Code", Name = "UQ__Product__A25C5AA733C60312", IsUnique = true)]
public partial class Product 
{
    [Key]
    [Column("ID")]

    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; }
    public Tager? Tag { get; set; } = null!;
    public int? TagId { get; set; }
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    [StringLength(100)]
    public string Brand { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    [Column("DESC")]
    public string Desc { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal? Discount { get; set; }

    public int? Stock { get; set; }

    [Required]
    [Column(TypeName = "bit")]
    public bool IsApproved { get; set; } = false;

   


    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    [Column("Sub_Category_ID")]
    public int? SubCategoryId { get; set; }

    [Column("Seller_ID")]
    public int? SellerId { get; set; }

    [InverseProperty("ProductCodeNavigation")]
    public virtual ICollection<CartProduct> CartProducts { get; set; } = new List<CartProduct>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    [InverseProperty("Product")]
    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    [InverseProperty("Product")]
    public virtual ICollection<Return> Returns { get; set; } = new List<Return>();

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [ForeignKey("SellerId")]
    [InverseProperty("Products")]
    public virtual Seller Seller { get; set; }

    [ForeignKey("TagId")]

    public virtual  Tag? TagObj { get; set; }


    [ForeignKey("SubCategoryId")]
    [InverseProperty("Products")]
    public virtual SubCategory SubCategory { get; set; }


    [ForeignKey("ProductId")]
    [InverseProperty("Products")]
    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}

public enum Tager
{
    feature = 1,
    Popular = 2,
    New = 3,
    BestSeller = 4,
    HotRelease = 5,
    BestDeal = 6,
    TopSelling = 7,
    Trending = 8,
}