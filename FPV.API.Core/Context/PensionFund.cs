using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

[Keyless]
public partial class PensionFund
{
    public int? Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public int? LinkingAmount { get; set; }

    public int? CategoryId { get; set; }
}
