using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Areas.Admin.ViewModels;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.Admin.Controllers.Items
{
    [Area("Admin")]
    [AdminSetupAuthorize()]
    public class ItemsController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IViewModelFactory _viewmodel;

        public ItemsController(ApplicationDbContext context,
                                IViewModelFactory viewmodel,
                                        IWebHostEnvironment hostEnvironment)
        {
            webHostEnvironment = hostEnvironment;
            _context = context;
            _viewmodel = viewmodel;
        }

        // GET: Admin/Items
        public async Task<IActionResult> Index(int? id)
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var list = _viewmodel.GetFilteredItemsByUserName(id, user);
            if(id != null)
            {
                var menu = await _context.Menu
                    .Where(m => m.MenuId == id)
                    .Select(m => new Menu()
                    {
                        MenuId = m.MenuId,
                        Name = m.Name
                    })
                    .FirstOrDefaultAsync();
                ViewBag.menu = menu;
            }
            return View(list);
        }

        // GET: Admin/Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var item = await _viewmodel.GetDetailItemAsync((int)id, user);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Admin/Items/Create
        public async Task<IActionResult> Create()
        {
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            ItemViewModel item = await _viewmodel.getItemRequirementsByUserName(user);
            return View(item);
        }

        // POST: Admin/Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemViewModel itemviewmodel)
        {
            if (ModelState.IsValid)
            {
                var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var pmax = 0;
                if (await _context.Item.Where(m => m.Menu.Restaurant.AdminUser.Id == user).AnyAsync())
                    pmax = await _context.Item.Where(m=>m.Menu.Restaurant.AdminUser.Id == user).Select(m => m.priority).MaxAsync();
                Item item = new Item()
                {
                    Menu = _context.Menu.Find(itemviewmodel.MenuId),
                    Category = _context.Category.Find(itemviewmodel.CategoryId),
                    Description = itemviewmodel.Description,
                    Price = itemviewmodel.Price,
                    Name = itemviewmodel.Name,
                    Image = itemviewmodel.Image,
                    isActive = itemviewmodel.Active,
                    isVegan = itemviewmodel.Type == 1,
                    isVegeterian = itemviewmodel.Type == 2,
                    Allergants = itemviewmodel.Allergants == null?null:await _context.Allergants.Where(m=> itemviewmodel.Allergants.Contains(m.AllergantId)).ToListAsync(),
                    Ingredients = itemviewmodel.Ingredients == null?null:await _context.Ingredients.Where(m=> itemviewmodel.Ingredients.Contains(m.Id)).ToListAsync(),
                    isAllergic = itemviewmodel.Allergants != null && itemviewmodel.Allergants.Count()>0,
                    priority = pmax + 1 
                };
                item.ImagePath = UploadedFile(item);
                _context.Add(item);
                await _context.SaveChangesAsync();
                return Redirect("/Admin/Items");
            }
            return View(itemviewmodel);
        }

        // GET: Admin/Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var item = await _viewmodel.GetEditItem((int)id, user);
            if (item == null)
            {
                return NotFound();
            }
            if (item.AllAllergants == null)
                item.AllAllergants = new List<Allergant>();
            if (item.Allergants == null)
                item.Allergants = new List<int>();
            return View(item);
        }

        // POST: Admin/Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemViewModel item)
        {
            Item NewItem = null;
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    NewItem = await _context.Item.Where(m => m.ItemId == id).Include(m=>m.Allergants).FirstOrDefaultAsync();
                    if (item.Image != null)
                    {
                        NewItem.Image = item.Image;
                        NewItem.ImagePath = UploadedFile(NewItem);
                    }
                    NewItem.Menu = _context.Menu.Find(item.MenuId);
                    NewItem.Category = _context.Category.Find(item.CategoryId);
                    NewItem.Price = item.Price;
                    NewItem.Description = item.Description;
                    NewItem.Name = item.Name;
                    var allergants = item.Allergants == null?null:await _context.Allergants.Where(m => item.Allergants.Contains(m.AllergantId)).ToListAsync();
                    NewItem.Allergants = allergants;
                    NewItem.isVegan = item.Type == 1;
                    NewItem.isAllergic = item.Allergants != null && item.Allergants.Count() > 0;

                    NewItem.isVegeterian = item.Type == 2;
                    NewItem.isActive = item.Active;
                    _context.Update(NewItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Items", new { area = "Admin" });
            }
            return View(item);
        }

        // GET: Admin/Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var item = await _viewmodel.GetItemAsync((int)id, user);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Admin/Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Items", new { area = "Admin" });
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }
        private string UploadedFile(Item item)
        {
            string uniqueFileName = null, filePath = null;

            if (item.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + item.Image.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    item.Image.CopyTo(fileStream);
                }
            }
            return "images/" + uniqueFileName;
        }
    }
}
