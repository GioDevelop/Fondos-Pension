using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class FundTransaction
{
    [Key]
    public int Id { get; set; }

    [StringLength(36)]
    public string? CustomerId { get; set; }

    public int? FundId { get; set; }

    public int? Value { get; set; }

    public int? TransactionTypeId { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TransactionDate { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("FundTransactions")]
    public virtual Customer? Customer { get; set; }

    [ForeignKey("FundId")]
    [InverseProperty("FundTransactions")]
    public virtual Fund? Fund { get; set; }

    [ForeignKey("TransactionTypeId")]
    [InverseProperty("FundTransactions")]
    public virtual TransactionType? TransactionType { get; set; }
}
