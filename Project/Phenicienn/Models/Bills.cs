using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class Bills
    {
        public int Id { get; set; }
        public table Table { get; set; }
        public string session { get; set; }
        public float Amount { get; set; }
        public bool Paid { get; set; }
    }
}
