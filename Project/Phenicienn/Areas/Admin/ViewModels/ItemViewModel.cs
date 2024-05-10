using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Phenicienn.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phenicienn.Areas.Admin.ViewModels
{
    public class ItemViewModel
    {
        [DisplayName("Menu")]
        public IEnumerable<Menu> menus { set; get; }
        [DisplayName("Category")]
        public IEnumerable<Category> categories { set; get; }
        [DisplayName("Allergants")]
        public List<Allergant> AllAllergants { set; get; }
        public List<Ingredient> AllIngredients { set; get; }
        public List<Allergant> DetailAllergants { set; get; }
        public int Id { set; get; }
        [Required]
        public int MenuId { set; get; }
        [Required]

        public int CategoryId { set; get; }
        [DisplayName("Menu")]

        public string MenuName { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; }
        [Required]

        public float Price { set; get; }
        [Required]

        public string Description { set; get; }
        [Required]

        public string Name { set; get; }
        [DisplayName("Image")]
        public string ImagePath { set; get; }
        public bool isVegan { set; get; }
        public bool isVegeterian { set; get; }
        [Required]

        public int Type { set; get; }
        [DefaultValue(true)]
        [DisplayName("Status")]
        public bool Active { set; get; }
        public bool Allergic { set; get; }
        public int priority { set; get; }
        [BindProperty]

        public List<int> Allergants { set; get; }
        public List<int> Ingredients { set; get; }

        [NotMapped]
        public IFormFile Image { set; get; }
    }
}
