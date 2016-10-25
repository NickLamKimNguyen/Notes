using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Common.Models.ValueObjects
{
    public class Price
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{Currency} {Amount}";
        }
    }
}
