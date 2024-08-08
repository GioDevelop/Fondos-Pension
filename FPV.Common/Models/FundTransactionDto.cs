using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Models
{
    public class FundTransactionDto
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }

        public int? FundId { get; set; }

        public int? Value { get; set; }

        public int? TransactionTypeId { get; set; }
        public string TransactionType { get; set; }

        public string? Description { get; set; }

        public DateTime? TransactionDate { get; set; }



    }
}
