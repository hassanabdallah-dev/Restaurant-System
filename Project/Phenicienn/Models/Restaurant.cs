using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Phenicienn.Models
{
    public class Restaurant
    {
        public int RestaurantId { set; get; }
        [Required]
        public string Name { set; get; }

        public IEnumerable<Menu> Menus { set; get; }

        public IEnumerable<table> tables { set; get; }
        [Required]
        public Phenicienn.Models.AdminUser.AdminUser AdminUser { set; get; }
    }
}
