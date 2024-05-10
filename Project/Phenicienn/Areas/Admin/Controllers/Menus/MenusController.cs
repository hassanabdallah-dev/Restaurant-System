using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Areas.Admin.ViewModels;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;

namespace Phenicienn.Areas.Admin.Controllers.Menus
{
    [Area("Admin")]
    [AdminSetupAuthorize()]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IViewModelFactory _viewmodel;
        private readonly UserManager<AdminUser> _userManager;
        public MenusController(ApplicationDbContext context
                                ,IViewModelFactory viewmodel,
                                    UserManager<AdminUser> userManager)
        {
            _context = context;
            _viewmodel = viewmodel;
            _userManager = userManager;

        }

        // GET: Admin/Menus
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _viewmodel.getMenusByUserName(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value));
        }

        // GET: Admin/Menus/Details/5
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Menu menu = null;
            var resto = await _context.Restaurant.Include(m => m.Menus).FirstOrDefaultAsync(m => m.RestaurantId == user.Restaurant);
            menu = resto.Menus.Where(m => m.MenuId == (int)id).FirstOrDefault();
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Admin/Menus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name)
        {
            string MenuName = Name;
            if (ModelState.IsValid)
            {
                var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var resto = await _context.Restaurant.FindAsync(user.Restaurant);
                var exists = await _context.Menu.AnyAsync(m => m.Restaurant.RestaurantId == resto.RestaurantId && m.Name == MenuName);
                if (!exists)
                {
                    Menu menu = new Menu()
                    {
                        Name = MenuName,
                        Restaurant = resto
                    };
                    _context.Add(menu);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Menus", new { area = "Admin" });
                }
            }
            return View();
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Menu menu = null;
            var resto = await _context.Restaurant.Include(m => m.Menus).FirstOrDefaultAsync(m =>m.RestaurantId == user.Restaurant);
            menu = resto.Menus.Where(m => m.MenuId == (int)id).FirstOrDefault();
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }
            var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resto = user.Restaurant;
            if(menu.Restaurant.RestaurantId != resto)
                return RedirectToAction("Index", "Menus", new { area = "Admin" });
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Menus", new { area = "Admin" });
            }
            return View(menu);
        }

        // GET: Admin/Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Menu menu = null;
            var resto = await _context.Restaurant.Include(m => m.Menus).FirstOrDefaultAsync(m => m.RestaurantId == user.Restaurant);
            if (resto.Menus != null && resto.Menus.Any(m => m.MenuId == (int)id))
                menu = await _viewmodel.getMenu((int)id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resto = await _context.Restaurant.Include(m => m.Menus).FirstOrDefaultAsync(m => m.RestaurantId == user.Restaurant);
            if (resto.Menus != null && resto.Menus.Any(m => m.MenuId == id))
            {
                var menu = await _context.Menu.FindAsync(id);
                _context.Menu.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Menus", new { area = "Admin" });
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.MenuId == id);
        }
    }
}
