using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Waiter.ViewModels
{
    public class WaiterItemViewModel
    {
        public int ItemId { set; get; }

        public float Price { set; get; }
        public string Name { set; get; }

        public string ImagePath { set; get; }
    }
}
