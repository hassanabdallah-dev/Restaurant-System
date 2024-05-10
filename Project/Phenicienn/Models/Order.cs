using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }
        public DateTime Date { get; set; }
        public float TotalPrice { get; set; }
        public String Commment { get; set; }
        public string session { set; get; }
        [DefaultValue(0)]
        public int Status { get; set; }
        [NotMapped]
        public bool Pending { get { return this.Status == 0; } }
        [NotMapped]
        public bool Validated { get { return this.Status == 1; } }
        [NotMapped]
        public bool Ready { get { return this.Status == 2; } }
        public table table { get; set; }
       
        public List<OrdersItems> OrdersItems { get; set; }
    }
}
