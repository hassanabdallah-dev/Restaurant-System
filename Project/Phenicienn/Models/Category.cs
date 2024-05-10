using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class Category
    {
        
        public int CategoryId { set; get; }
        [Required]
        public string Name { set; get; }
        [DisplayName("Image")]
        public string ImagePath { set; get; }
        [NotMapped]
        [Required]
        public IFormFile Image { set; get; }
        [Required]
        public string owner { set; get; }

        [DefaultValue(0)]
        public int priority { set; get; }
   

    }
}
