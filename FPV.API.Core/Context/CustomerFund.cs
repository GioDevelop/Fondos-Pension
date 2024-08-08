using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class CustomerFund
{
    [Key]
    [StringLength(36)]
    public string Id { get; set; } = null!;

    [StringLength(36)]
    public string CustomerId { get; set; } = null!;

    public int FundId { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("CustomerFunds")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("FundId")]
    [InverseProperty("CustomerFunds")]
    public virtual Fund Fund { get; set; } = null!;
}
