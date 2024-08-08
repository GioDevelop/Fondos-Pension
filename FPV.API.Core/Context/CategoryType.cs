using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FPV.API.Core.Context;

public partial class CategoryType
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? Name { get; set; }

    public bool? Active { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Fund> Funds { get; set; } = new List<Fund>();
}
