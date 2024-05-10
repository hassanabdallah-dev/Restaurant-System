using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Kitchen.ViewModels
{
    public class KitchenOrderViewModel
    {
        public int Id { get; set; }
        public int TableNo { get; set; }
        public string Comment { get; set; }
        public List<KitchenItemViewModel> items { set; get; }
    }
}
