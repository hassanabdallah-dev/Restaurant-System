using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models.AdminUser;
using SQLitePCL;

namespace Phenicienn.Areas.Manage.Controllers.Users
{
    [Area("Manage")]
    [SuperAdminAuthorization()]

    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AdminUser> _userManager;

        public UsersController(ApplicationDbContext context,
            UserManager<AdminUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.AdminUser.ToListAsync();
            List<AdminUser> toDisplay = new List<AdminUser>();
            foreach(var user in users)
            {
                if (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                {
                    toDisplay.Add(user);
                    if(user.Restaurant != 0)
                    {
                        ViewData["resto-" + user.Restaurant] = (await _context.Restaurant.FindAsync(user.Restaurant)).Name;
                    }
                }
            }
            return View(toDisplay);
        }
        [HttpPost]
        public async Task<IActionResult> Details(string id) 
        {
            if (id == null)
                return NotFound();
            var user = await _context.AdminUser
                .Select(m => new AdminUser { 
                    Id = m.Id,
                    UserName = m.UserName,
                    active = m.active
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if(!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                return View(user);
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var user = await _context.AdminUser
                .Select(m => new AdminUser
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    active = m.active
                })
                .FirstOrDefaultAsync(m => m.Id == id);
            if (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                return View(user);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Activate(string id)
        {
            if (id == null)
                return NotFound();
            var user = await _context.AdminUser.FirstOrDefaultAsync(m => m.Id == id);
            if (!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
            {
                user.active = true;
                await _context.SaveChangesAsync();
            }
            return Redirect("/Manage/Users");
        }
    }
}