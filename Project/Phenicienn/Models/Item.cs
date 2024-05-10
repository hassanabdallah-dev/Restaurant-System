using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Models
{
    public class Item
    {
        public int ItemId { set; get; }

        [Required]
        [ForeignKey("MenuFK")]
        public Menu Menu { set; get; }
        public int MenuFK { get; set; }
        [Required]
        [ForeignKey("cateforyFK")]
        public Category Category { set; get; }
        public int cateforyFK { get; set; }
        [Required]

        public float Price { set; get; }
        [Required]

        public string Description { set; get; }
        [Required]

        public string Name { set; get; }

        public string ImagePath { set; get; }
        [NotMapped]
        [Required]
        public IFormFile Image { set; get; }
        [Required]
        [DefaultValue(true)]

        public bool isVegan { set; get; }
        [Required]
        [DefaultValue(true)]

        public bool isVegeterian { set; get; }
        [DefaultValue(true)]
        public bool isActive { set; get; }
        [Required]
        [DefaultValue(false)]

        public bool isAllergic { set; get; }
        public List<Allergant> Allergants { set; get; }
        public List<Ingredient> Ingredients { set; get; }


        [DefaultValue(0)]

        public int priority { set; get; }
    }
}
