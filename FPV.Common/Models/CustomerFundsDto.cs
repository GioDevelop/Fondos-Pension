using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Models
{
    public class CustomerFundsDto
    {
        public string Id { get; set; } = null!;

        public string CustomerId { get; set; } = null!;

        public int FundId { get; set; }

        public  FundDto Fund { get; set; } = null!;
    }
}
