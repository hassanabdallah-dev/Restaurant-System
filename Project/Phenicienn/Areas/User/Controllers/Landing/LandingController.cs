using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Areas.User.ViewModels;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.User.Controllers.Landing
{
    [Area("User")]
    public class LandingController : Controller
    {
        private readonly ILogger<LandingController> _logger;
        private readonly IViewModelFactory _viewmodel;
        private readonly ApplicationDbContext context;

        public LandingController(ILogger<LandingController> logger,
                                IViewModelFactory viewmodel, ApplicationDbContext context)
        {
            _logger = logger;
            _viewmodel = viewmodel;
            this.context = context;
        }

        public IActionResult Index(string restaurant)
        {
            
            return View();
        }
        [HttpPost]
        public List<Item> getCatItems(int id)
        {

            Menu menu =context.Menu.Where(q => q.RestaurantFK == HttpContext.Session.GetInt32("res_id")).FirstOrDefault();

            return context.Item.Where(q=>q.cateforyFK==id && q.MenuFK==menu.MenuId).ToList();
        }
        [HttpPost]
        public List<Item> getAllItems ()
        {
            Menu menu = context.Menu.Where(q => q.RestaurantFK == (int)HttpContext.Session.GetInt32("res_id")).FirstOrDefault();
            return context.Item.Where(q=> q.MenuFK == menu.MenuId).ToList();
        }

        [HttpPost]
        public  IngredientsAllergants getAllergantItems(int id)
        {
            var allergants= context.Item.Include(m => m.Allergants).FirstOrDefault(m=>m.ItemId==id).Allergants;
            var ingredients= context.Item.Include(m => m.Ingredients).FirstOrDefault(m=>m.ItemId==id).Ingredients;
            return new IngredientsAllergants() { 
                Ingredients = ingredients,
                Allergants = allergants
            };
        }



        [HttpGet]
        public async Task<IActionResult> Menu(int? cat)
        {


            var resid = HttpContext.Session.GetInt32("res_id");
            if (resid == null)
                return Redirect("/User/QRCodeRedirection");

            var list = await _viewmodel.GetUserItems((int)resid);
            // var list = context.Menu.Where(q => q.RestaurantFK == id);
            Dictionary<int, string> cats = new Dictionary<int, string>();
            
            foreach(var item in list)
            {
                Category c = new Category();
                c = context.Category.Find(item.cateforyFK);
                if(!cats.ContainsKey(c.CategoryId))
                {
                    cats.Add(item.cateforyFK, c.Name);
                }
            }
            ViewBag.cats = cats;
            return View(list);
        }
        /*public async Task<IActionResult> Menu(int id)
        {
            var list1 = await _viewmodel.GetUsercats(id);
            var list = await _viewmodel.GetUserItems(null);
            return View(list);
        }*/
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
