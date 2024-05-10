using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Waiter.ViewModels
{
    public class WaiterOrdersItemsViewModel
    {
        public int Id { get; set; }
        public WaiterItemViewModel Item { get; set; }
        public int Quantity { get; set; }

    }
}
