using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class Fund
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public int? LinkingAmount { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Funds")]
    public virtual CategoryType? Category { get; set; }

    [InverseProperty("Fund")]
    public virtual ICollection<CustomerFund> CustomerFunds { get; set; } = new List<CustomerFund>();

    [InverseProperty("Fund")]
    public virtual ICollection<FundTransaction> FundTransactions { get; set; } = new List<FundTransaction>();
}
