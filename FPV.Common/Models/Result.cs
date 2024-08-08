using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPV.Common.Models
{
    public class Result<T>
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public T? Value { get; set; }
    }
}
