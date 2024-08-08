using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Models
{
    public class FundDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? LinkingAmount { get; set; }

        public int? CategoryId { get; set; }
    }
}
