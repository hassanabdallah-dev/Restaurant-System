using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phenicienn.DataInitializers
{
    public class IdentityDataInitializer
    {
        public static async void SeedData(UserManager<AdminUser> userManager,
                                        RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        public static async Task SeedUsers(UserManager<AdminUser> userManager)
        {
            if(userManager.FindByNameAsync("lephenicienroot").Result == null)
            {
                AdminUser admin = new AdminUser() {
                    UserName = "lephenicienroot",
                    active = true,
                };
                IdentityResult result = userManager.CreateAsync(admin, "@Le.Phen.Root@1").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin,"SuperAdmin");
                    await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.NameIdentifier, admin.Id));
                    await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Role, "SuperAdmin"));
                }

            }
            if(userManager.FindByNameAsync("lephenicienroot1").Result == null)
            {
                AdminUser admin = new AdminUser() {
                    UserName = "lephenicienroot1",
                    active = true,
                };
                IdentityResult result = userManager.CreateAsync(admin, "@Le.Phen.Root@1").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin,"SuperAdmin");
                    await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.NameIdentifier, admin.Id));
                    await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Role, "SuperAdmin"));
                }

            }
        }

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Admin";
                await roleManager.CreateAsync(role);
            }


            if (!roleManager.RoleExistsAsync("SuperAdmin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "SuperAdmin";
                await roleManager.CreateAsync(role);
            }
        }
    }
}
