using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class TransactionType
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    [InverseProperty("TransactionType")]
    public virtual ICollection<FundTransaction> FundTransactions { get; set; } = new List<FundTransaction>();
}
