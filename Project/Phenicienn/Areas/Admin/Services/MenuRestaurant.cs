using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.ViewModels;
using Phenicienn.Areas.Waiter.ViewModels;
using Phenicienn.Data;
using Phenicienn.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.Services
{
    public class MenuRestaurant : IViewModelFactory
    {
        private readonly ApplicationDbContext _context;
        public MenuRestaurant(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<MenuRestaurantViewModel>> getRestaurants()
        {
            List<MenuRestaurantViewModel> list = await _context.Restaurant.Select(m => new MenuRestaurantViewModel { 
                RestaurantId = m.RestaurantId,
                RestaurantName = m.Name
            }).ToListAsync();
            return list;
        }
        public string TimeAgo(DateTime dateTime)
        {
            if (DateTime.MinValue == dateTime)
                return "";
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("{0} minutes ago", timeSpan.Minutes) :
                    "1 minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("{0} hours ago", timeSpan.Hours) :
                    "1 hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("{0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("{0} months ago", timeSpan.Days / 30) :
                    "1 month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("{0} years ago", timeSpan.Days / 365) :
                    "1 year ago";
            }

            return result;
        }
        public int getTablePendingOrdersCount(int id)
        {
            return _context.Orders
                .Where(o => o.table.id == id)
                .Where(m => m.Status == 0)
                .Where(m => m.session == m.table.session)
                .Count();
        }
        public DateTime getTableLastOrderDate(int id)
        {
            return _context.Orders
                .Where(o => o.table.id == id)
                .Where(m => m.session == m.table.session)
                .Where(m => m.Status == 0)
                .OrderByDescending(m=>m.Date)
                .Select(m => m.Date)
                .FirstOrDefault();
        }
        public async Task<List<Menu>> getMenusByUserName(string user)
        {
            List<Menu> list = await _context.Menu
                .Where(m => m.Restaurant.AdminUser.Id == user)
                .Select(m => new Menu
            {
                MenuId = m.MenuId,
                Name = m.Name,
            }).ToListAsync();
            return list;
        }
        public async Task<Menu> getMenu(int id)
        {
            var item = await _context.Menu.Where(m => m.MenuId == id)
                .Select(m=> new Menu { 
                    MenuId = m.MenuId,
                    Name = m.Name
                    })
                .FirstOrDefaultAsync();

            return item;
        }
        public IEnumerable<MenuRestaurantViewModel> getMenus()
        {
            /*List<MenuRestaurantViewModel> */
            IEnumerable<MenuRestaurantViewModel> list = from m in _context.Menu
                                                 join r in _context.Restaurant
                                                 on m.Restaurant.RestaurantId equals r.RestaurantId
                                                 //on m.MenuId equals r.RestaurantId
                                                        select new MenuRestaurantViewModel
                                                 {
                                                     MenuId = m.MenuId,
                                                     MenuName = m.Name,
                                                     RestaurantId = r.RestaurantId,
                                                     RestaurantName = r.Name
                                                 };
/*                .Join(
                _context.Restaurant,
                menu => menu.Id,
                restaurant => restaurant.
                (menu, restaurant) => new MenuRestaurantViewModel
                {
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    RestaurantId = m.RestaurantId,
                }).ToListAsync();*/
            return list;
        }
        public async Task<IEnumerable<Menu>> GetAllMenus()
        {
            var menu = await _context.Menu.Select(m => new Menu
            {
                MenuId = m.MenuId,
                Name = m.Name
            }).ToListAsync();
            return menu;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserName(string user)
        {
            var cat = await _context.Category
                .Where(m=>m.owner == user)
                .Select(m => new Category
            {
                CategoryId = m.CategoryId,
                Name = m.Name
            }).ToListAsync();
            return cat;
        }
        public async Task<List<Allergant>> getAllergants()
        {
            var list = await _context.Allergants.ToListAsync();
            return list;
        }
        public async Task<ItemViewModel> getItemRequirementsByUserName(string user)
        {
            ItemViewModel item = new ItemViewModel() {
                menus = await getMenusByUserName(user),
                categories = await GetCategoriesByUserName(user),
                AllAllergants = await getAllergants(),
                AllIngredients = _context.Ingredients.ToList()
                
            };
            return item;
        }
        public IEnumerable<ItemViewModel> GetFilteredItemsByUserName(int? id, string user)
        {
            IEnumerable<ItemViewModel> list;
            if (id == null)
            {
                list = from i in _context.Item
                                                  join m in _context.Menu
                                                  on i.Menu.MenuId equals m.MenuId
                                                  join c in _context.Category
                                                  on i.Category.CategoryId equals c.CategoryId
                                                  where m.Restaurant.AdminUser.Id == user
                                                  select new ItemViewModel
                                                  {
                                                      Id = i.ItemId,
                                                      MenuId = m.MenuId,
                                                      CategoryId = c.CategoryId,
                                                      MenuName = m.Name,
                                                      CategoryName = c.Name,
                                                      Price = i.Price,
                                                      Description = i.Description,
                                                      Name = i.Name,
                                                      ImagePath = i.ImagePath,
                                                      isVegan = i.isVegan,
                                                      isVegeterian = i.isVegeterian,
                                                      Active = i.isActive,
                                                      Allergic = i.isAllergic,
                                                      priority = i.priority
                                                  };
            }
            else
            {
                list = from i in _context.Item
                                                  join c in _context.Category
                                                  on i.Category.CategoryId equals c.CategoryId
                                                  where i.Menu.MenuId == (int)id
                                                  where i.Menu.Restaurant.AdminUser.Id == user
                       select new ItemViewModel
                                                  {
                                                      Id = i.ItemId,
                                                      CategoryId = c.CategoryId,
                                                      CategoryName = c.Name,
                                                      Price = i.Price,
                                                      Description = i.Description,
                                                      Name = i.Name,
                                                      ImagePath = i.ImagePath,
                                                      isVegan = i.isVegan,
                                                      isVegeterian = i.isVegeterian,
                                                      Allergic = i.isAllergic,
                                                      Active = i.isActive,
                                                      priority = i.priority
                       };
            }
            var a = list.Count();
            return list.OrderBy(m => m.priority).ToList();
        }
        public IEnumerable<ItemViewModel> GetItemsByUserName(string user)
        {
            IEnumerable<ItemViewModel> list = from i in _context.Item
                                              join m in _context.Menu
                                              on i.Menu.MenuId equals m.MenuId
                                              join c in _context.Category
                                              on i.Category.CategoryId equals c.CategoryId
                                              where  m.Restaurant.AdminUser.Id == user
                                              select new ItemViewModel
                                              {
                                                  Id = i.ItemId,
                                                  MenuId = m.MenuId,
                                                  CategoryId = c.CategoryId,
                                                  MenuName = m.Name,
                                                  CategoryName = c.Name,
                                                  Price = i.Price,
                                                  Description = i.Description,
                                                  Name = i.Name,
                                                  ImagePath = i.ImagePath
                                              };
            return list;
        }
        public async Task<ItemViewModel> GetItemAsync(int id, string user)
        {
            var Iallergants = await _context.Item.Where(m => m.ItemId == id).Select(m => m.Allergants.Select(s => s.AllergantId)).FirstOrDefaultAsync();
            var allergants = Iallergants.ToList();
            var item = from i in _context.Item
                                              join m in _context.Menu
                                              on i.Menu.MenuId equals m.MenuId
                                              join c in _context.Category
                                              on i.Category.CategoryId equals c.CategoryId
                                              where i.ItemId == id
                                              where i.Menu.Restaurant.AdminUser.Id == user
                                              select new ItemViewModel
                                              {
                                                  Id = i.ItemId,
                                                  MenuId = m.MenuId,
                                                  CategoryId = c.CategoryId,
                                                  MenuName = m.Name,
                                                  CategoryName = c.Name,
                                                  Price = i.Price,
                                                  Description = i.Description,
                                                  Name = i.Name,
                                                  ImagePath = i.ImagePath,
                                                  isVegan = i.isVegan,
                                                  isVegeterian = i.isVegeterian,
                                                  Allergic = i.isAllergic,
                                                  Allergants = allergants,
                                                  Active = i.isActive,
                                                  priority = i.priority
                                              };
            ItemViewModel t = item.FirstOrDefault();
            return t;
        }
        public async Task<ItemViewModel> GetDetailItemAsync(int id, string user)
        {
            var Iallergants = await _context.Item.Where(m => m.ItemId == id).Select(m => m.Allergants).FirstOrDefaultAsync();
            var allergants = Iallergants.ToList();
            var item = from i in _context.Item
                                              join m in _context.Menu
                                              on i.Menu.MenuId equals m.MenuId
                                              join c in _context.Category
                                              on i.Category.CategoryId equals c.CategoryId
                                              where i.ItemId == id
                                              where i.Menu.Restaurant.AdminUser.Id == user
                                              select new ItemViewModel
                                              {
                                                  Id = i.ItemId,
                                                  MenuId = m.MenuId,
                                                  CategoryId = c.CategoryId,
                                                  MenuName = m.Name,
                                                  CategoryName = c.Name,
                                                  Price = i.Price,
                                                  Description = i.Description,
                                                  Name = i.Name,
                                                  ImagePath = i.ImagePath,
                                                  isVegan = i.isVegan,
                                                  isVegeterian = i.isVegeterian,
                                                  Allergic = i.isAllergic,
                                                  DetailAllergants = allergants,
                                                  Active = i.isActive,
                                                  priority = i.priority
                                              };
            ItemViewModel t = item.FirstOrDefault();
            return t;
        }
        public async Task<ItemViewModel> GetEditItem(int id, string user)
        {
            var item = await GetItemAsync(id, user);
            if (item == null)
                return null;
            var req = await getItemRequirementsByUserName(user);
            item.AllAllergants = req.AllAllergants;
            item.categories = req.categories;
            item.menus = req.menus;
            return item;
        }
        public WaiterItemViewModel SelectForOrder(Item item)
        {
            return new WaiterItemViewModel() {
                ItemId = item.ItemId,
                Price = item.Price,
                Name = item.Name,
                ImagePath = item.ImagePath
            };
        }
        public async Task<List<Item>> GetUserItems(int cat)
        {
            int? id = cat;

            if (id == null)
            {
               var i = await _context.Item.Select(m => new Item()
                {
                    ItemId = m.ItemId,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImagePath = m.ImagePath
                })
                .ToListAsync();
                return i;
            }
            var menu = _context.Menu.Where(q => q.RestaurantFK == id).FirstOrDefault();

            var items = _context.Item
                .Where(m => m.MenuFK == menu.MenuId)
                .ToList();

            return items;
        }


        public async Task<List<Category>> GetUserCategories()
        {
            /*var list = from c in _context.Category
                       join f in _context.CategoryFilters
                       on c.CategoryId equals f.Category.CategoryId
                       select new UserCategory() { Name = c.Name, Id = f.CategoryFilterId,
                           Count = (from q1 in _context.Item
                                    where q1.Category.CategoryId == c.CategoryId
                                    select q1).Count()
                       };
            var l = await list.ToListAsync();
            return l;*/
            var list = await _context.Category.ToListAsync();
            return list;
        }

    }
}
