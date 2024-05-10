using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;
using Phenicienn.Models.User;

namespace Phenicienn.Areas.Account.Controllers.Register
{
    [Area("Account")]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AdminUser> _usermanager;
        private readonly SignInManager<AdminUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RegisterController(ApplicationDbContext context,
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
        public async Task<IActionResult> Index([FromForm] RegisterUser registerUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _usermanager.FindByNameAsync(registerUser.UserName);
                if (user == null)
                {
                    var iduser = new AdminUser()
                    {
                        UserName = registerUser.UserName

                    };
                    var succeed = await _usermanager.CreateAsync(iduser, registerUser.Password);
                    if (succeed.Succeeded)
                    {
                        await _usermanager.AddClaimAsync(iduser, new Claim(ClaimTypes.NameIdentifier, iduser.Id));
                        await _usermanager.AddClaimAsync(iduser, new Claim(ClaimTypes.Role, "Admin"));
                        await _usermanager.AddClaimAsync(iduser, new Claim("setup", "false"));
                        HttpContext.Session.SetString("setup", "false");
                        bool success = await _roleManager.RoleExistsAsync("Admin");
                        if (!success)
                        {
                            IdentityRole role = new IdentityRole();
                            role.Name = "Admin";
                            await _roleManager.CreateAsync(role);
                        }
                        await _usermanager.AddToRoleAsync(iduser, "Admin");
                        var signedIn = await _signInManager.PasswordSignInAsync(iduser, iduser.PasswordHash, false, false);
                        if (signedIn.Succeeded)
                        {

                        }
                        else
                        {

                        }
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
            return Redirect("/Account/Login");
        }
    }
}