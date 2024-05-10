using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class CategoryFilter
    {
        public int CategoryFilterId { get; set; }
        [Required]
        [ForeignKey("FK_CATEGORYFILTER_CATEGORY")]
        public Category Category { get; set; }
       public int FK_CATEGORYFILTER_CATEGORY { get; set; }
    }
}
