using Phenicienn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.User.ViewModels
{
    public class IngredientsAllergants
    {
        public List<Allergant> Allergants { set; get; }
        public List<Ingredient> Ingredients { get; set; }

    }
}
