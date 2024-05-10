using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class Menu
    {
        public int MenuId { set; get; }
        [Required]
        public string Name { get; set; }
        [Required]
        [ForeignKey("RestaurantFK")]
        public Restaurant Restaurant { set; get; }
        public int RestaurantFK { get; set; }
        
       
    }
}
