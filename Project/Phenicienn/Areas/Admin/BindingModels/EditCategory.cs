using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.BindingModels
{
    public class EditCategory
    {
        public int CategoryId { set; get; }
        [Required]
        public string Name { set; get; }
        public IFormFile Image { set; get; }
    }
}
