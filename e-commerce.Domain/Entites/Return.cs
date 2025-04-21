using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using e_commerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Infrastructure.Entites;

public partial class Return
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public string Reason { get; set; }

    [Required]
    public ReturnStatusEnum Status { get; set; }

    [Column("Order_ID")]
    public int OrderId { get; set; }

    [Column("Product_ID")]
    public int ProductId { get; set; }
    [ForeignKey("Customer")]
    public int custId { get; set; }
    public  Customer? Customer { get; set; }

    public decimal AmountRefunded { get; set; }

    [Column("Return_Date", TypeName = "datetime")]
    public DateTime? ReturnDate { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Returns")]
    public virtual Order Order { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Returns")]
    public virtual Product Product { get; set; }

}