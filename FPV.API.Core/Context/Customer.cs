using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

[Table("Customer")]
public partial class Customer
{
    [Key]
    [StringLength(36)]
    public string Id { get; set; } = null!;

    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<CustomerFund> CustomerFunds { get; set; } = new List<CustomerFund>();

    [InverseProperty("Customer")]
    public virtual ICollection<FundTransaction> FundTransactions { get; set; } = new List<FundTransaction>();
}
