using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;
using Phenicienn.Models.User;

namespace Phenicienn.Areas.Admin.Controllers.Employees
{
    [Area("Admin")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AdminUser> _usermanager;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EmployeesController(ApplicationDbContext context,
            UserManager<AdminUser> userManager,
                                      SignInManager<AdminUser> signInManager,
                                      RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _usermanager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] RegisterUser registerUser, int type=1)
        {
            if (ModelState.IsValid)
            {
                var rolename = type == 1 ? "Waiter" : (type == 2 ?"Cachier":(type == 3?"Kitchen":""));
                if(rolename == "")
                {
                    List<string> errors = new List<string>();
                    errors.Add("Input Error!");
                    ViewBag.Errors = errors;
                    return View();
                }
                var user = await _usermanager.FindByNameAsync(registerUser.UserName);
                if (user == null)
                {
                    var RestaurantId = await _context.AdminUser
                        .Where(m => m.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                        .Select(m => m.Restaurant).FirstOrDefaultAsync();
                    var iduser = new AdminUser()
                    {
                        UserName = registerUser.UserName,
                        Restaurant = RestaurantId
                        
                    };
                    var succeed = await _usermanager.CreateAsync(iduser, registerUser.Password);
                    if (succeed.Succeeded)
                    {
                        await _usermanager.AddClaimAsync(iduser, new Claim(ClaimTypes.NameIdentifier, iduser.Id));
                        await _usermanager.AddClaimAsync(iduser, new Claim(ClaimTypes.Role, rolename));
                        bool success = await _roleManager.RoleExistsAsync(rolename);
                        if (!success)
                        {
                            IdentityRole role = new IdentityRole();
                            role.Name = rolename;
                            await _roleManager.CreateAsync(role);
                        }
                        await _usermanager.AddToRoleAsync(iduser, rolename);
                        
                    }
                }
                else
                {
                    List<string> errors = new List<string>();
                    errors.Add("Unfortunately, This Username has been taken!");
                    ViewBag.Errors = errors;
                    return View();
                }
            }
            return Redirect("/Admin");
        }
    }
}