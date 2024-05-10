using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class table
    { 
        [Key] 
        public int id { get; set; }
        [NotMapped]
        public bool ai { set; get; }
        [DefaultValue(0)]
        public int TableNo { set; get; }
        public Reservation Reservation { get; set; }
        [DefaultValue(false)]

        [Required]
        public int capacity { set; get; }
        [DefaultValue(true)]
        public bool available { get; set; }
        public string key { get; set; }

        public string session { get; set; }

        [ForeignKey("RestaurantFK")]
        public Restaurant Restaurant { set; get; }
        public int RestaurantFK { get; set; }

    }
}
