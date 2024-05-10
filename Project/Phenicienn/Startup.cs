using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Models;
using Phenicienn.Data;
using System.Security.Policy;
using Phenicienn.DataInitializers;
using Microsoft.CodeAnalysis.Options;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Phenicienn.Models.AdminUser;
using Microsoft.AspNetCore.Routing.Template;

namespace Phenicienn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IViewModelFactory, MenuRestaurant>();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddRazorPages().AddRazorPagesOptions(options => {
                options.Conventions.AuthorizeFolder("/");
            });

            services.AddAuthorization();
            services.AddIdentity<AdminUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.Configure<IdentityOptions>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Manage";

            });
            services.AddSession();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
                                IWebHostEnvironment env,
                                   UserManager<AdminUser> userManager,
                                        RoleManager<IdentityRole> roleManager) 
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               // app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
          
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            
            IdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapAreaControllerRoute(
                    name: "default",
                    areaName: "User",
                    pattern: "{area:exists}/{controller=Landing}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "user",
                    areaName: "User",
                    pattern: "{area:exists}/{controller=Landing}/{action=Index}/{id}/{table_id}/{cat_id?}",
                    defaults: new {area = "User", controller="Landing", action="Index", restaurant="main"}
                    );
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "{area:exists}/{controller=Items}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "manage",
                    areaName: "Manage",
                    pattern: "{area:exists}/{controller=Users}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "account",
                    areaName: "Account",
                    pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "waiter",
                    areaName: "Waiter",
                    pattern: "{area:exists}/{controller=ManageTables}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "cachier",
                    areaName: "Cachier",
                    pattern: "{area:exists}/{controller=Bills}/{action=Index}/{id?}"
                    );
                endpoints.MapAreaControllerRoute(
                    name: "kitchen",
                    areaName: "Kitchen",
                    pattern: "{area:exists}/{controller=IncomingOrders}/{action=Index}/{id?}"
                    );

                endpoints.MapRazorPages();
            });
        }
    }
}
