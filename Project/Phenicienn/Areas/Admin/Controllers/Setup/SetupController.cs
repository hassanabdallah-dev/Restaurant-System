using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;
using Phenicienn.Models.AdminUser;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.Controllers.Setup
{
    [Area("Admin")]
    [AdminSetupReverseAuthorize()]
    public class SetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AdminUser> _userManager;

        public SetupController(ApplicationDbContext context,
                                    UserManager<AdminUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var message = HttpContext.Session.GetString("ErrorMessage");
            if (message != null && message != "")
            {
                ViewData["ErrorMessage"] = message;
                HttpContext.Session.Remove("ErrorMessage");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index([FromForm] Restaurant restaurant)
        {
            if (restaurant.Name != null && restaurant.Name != "")
            {
                if (await _context.Restaurant.Where(m => m.Name == restaurant.Name).FirstOrDefaultAsync() != null)
                    return View(restaurant);
                var id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var user = await _context.AdminUser.FindAsync(id);
                restaurant.AdminUser = user;
                await _context.Restaurant.AddAsync(restaurant);
                await _context.SaveChangesAsync();
                user.Restaurant = restaurant.RestaurantId;
                _context.AdminUser.Update(user);
                await _context.SaveChangesAsync();
                Menu menu = new Menu()
                {
                    Name=restaurant.Name+ " Menu",
                    Restaurant = restaurant,
                    RestaurantFK = restaurant.RestaurantId,
                };
                await _context.Menu.AddAsync(menu);
                await _context.SaveChangesAsync();
                await _userManager.RemoveClaimAsync(user, HttpContext.User.FindFirst("setup"));
                await _userManager.AddClaimAsync(user, new Claim("setup", "true"));
                HttpContext.Session.SetString("setup", "true");
                user.active = true;
                await _context.SaveChangesAsync();
                return Redirect("/Admin");
            }
            return View(restaurant);
        }
    }
}