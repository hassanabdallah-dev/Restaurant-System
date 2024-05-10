using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Waiter.ViewModels
{
    public class TableViewModel
    {
        public int Id { get; set; }
        public int capacity { get; set; }
        public int status { get; set; }
        public int TableNo { get; set; }
        public string LastOrder { get; set; }
        public int PendingOrdersCount { get; set; }
        public string Reservation { get; set; }
        public string Name { get; set; }
        public int nb_people { get; set; }
    }
}
