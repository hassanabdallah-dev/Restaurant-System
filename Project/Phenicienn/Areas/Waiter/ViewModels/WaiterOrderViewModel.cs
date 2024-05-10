using Phenicienn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Waiter.ViewModels
{
    public class WaiterOrderViewModel
    {
        public int Order_Id { get; set; }
        public DateTime Date { get; set; }
        public string HumanDate { get; set; }
        public string session { get; set; }
        public List<WaiterOrdersItemsViewModel> OrdersItems { get; set; }
    }
}
