using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.ViewModels
{
    public class MenuViewModel
    {
        public IEnumerable<MenuRestaurantViewModel> restaurants;
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int RestaurantId { get; set; }
    }
}
