using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.ViewModels
{
    public class MenuRestaurantViewModel
    {
        public int RestaurantId { get; set; }
        public int MenuId { get; set; }
        public string RestaurantName { get; set; }
        public string MenuName { get; set; }
    }
}
