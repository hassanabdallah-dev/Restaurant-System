using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;
using Phenicienn.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;


namespace Phenicienn.Areas.Account.Controllers.Login
{
    [Area("Account")]
    public class LoginController : Controller
    {
        UserManager<AdminUser> _userManager;
        SignInManager<AdminUser> _signinManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public LoginController(UserManager<AdminUser> userManager,
                                SignInManager<AdminUser> signinManager,
                                RoleManager<IdentityRole> roleManager,
                                ApplicationDbContext context)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = User.FindFirst(ClaimTypes.Role).Value;
                if (role == "Admin")
                    return Redirect("/Admin");
                else if (role == "SuperAdmin")
                    return Redirect("/Manage");
                else if (role == "Waiter")
                    return Redirect("/Waiter");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginUser user)
        {
            if (User.Identity.IsAuthenticated)
            {
                var rolee = User.FindFirst(ClaimTypes.Role).Value;
                if (rolee == "Admin")
                    return Redirect("/Admin");
                else if (rolee == "SuperAdmin")
                    return Redirect("/Manage");
                else
                    return Redirect("/");

            }
            List<string> errors = null;
            var iduser = await _userManager.FindByNameAsync(user.Username);
            if (iduser == null)
            {
                errors = new List<string>();
                errors.Add("Wrong Credentials or Account not approved yet");
                ViewBag.Errors = errors;
                return View();
            }
            else if (!iduser.active)
            {
                errors = new List<string>();
                errors.Add("Wrong Credentials or Account not approved yet");
                ViewBag.Errors = errors;
                return View();
            }
            var admin = await _userManager.IsInRoleAsync(iduser, "Admin");
            var superadmin = false;
            var waiter = false;
            if (!admin)
            {
                superadmin = await _userManager.IsInRoleAsync(iduser, "SuperAdmin");
                if (!superadmin)
                    waiter = await _userManager.IsInRoleAsync(iduser, "Waiter") || await _userManager.IsInRoleAsync(iduser, "Kitchen") || await _userManager.IsInRoleAsync(iduser, "Cachier");
            }
            var role = admin || superadmin || waiter;
            if (iduser != null && role)
            {
                var result = await _signinManager.PasswordSignInAsync(iduser, user.Password, false, true);
                if (!result.Succeeded)
                {
                    errors = new List<string>();
                    errors.Add("Wrong Credentials or Account not approved yet");
                    ViewBag.Errors = errors;
                    return View();
                }
                else
                {

                    HttpContext.Session.SetString("setup", iduser.setup ? "true" : "false");
                }
            }
            else
            {
                errors = new List<string>();
                errors.Add("Wrong Credentials or Account not approved yet");
                ViewBag.Errors = errors;
                return View();
            }
            if (admin)
                return Redirect("/Admin");
            else if (superadmin)
                return Redirect("/Manage");
            else if (waiter)
                return Redirect("/Waiter");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return Redirect("/Account/Login");
        }
    }
}
