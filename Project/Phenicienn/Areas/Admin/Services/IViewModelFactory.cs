using Phenicienn.Areas.Admin.ViewModels;
using Phenicienn.Areas.Waiter.ViewModels;
using Phenicienn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Phenicienn.Areas.Admin.Services
{
    public interface IViewModelFactory
    {
        IEnumerable<MenuRestaurantViewModel> getMenus();
        Task<List<MenuRestaurantViewModel>> getRestaurants();
        Task<Menu> getMenu(int id);
        Task<ItemViewModel> getItemRequirementsByUserName(string user);
        IEnumerable<ItemViewModel> GetFilteredItemsByUserName(int? id, string user);
        Task<IEnumerable<Menu>> GetAllMenus();
        Task<IEnumerable<Category>> GetCategoriesByUserName(string user);
        Task<ItemViewModel> GetEditItem(int id, string user);
        Task<List<Item>> GetUserItems(int id);
    
        Task<List<Category>> GetUserCategories();

        Task<List<Menu>> getMenusByUserName(string user);
        IEnumerable<ItemViewModel> GetItemsByUserName(string user);
        Task<List<Allergant>> getAllergants();
        Task<ItemViewModel> GetItemAsync(int id, string user);
        Task<ItemViewModel> GetDetailItemAsync(int id, string user);
        int getTablePendingOrdersCount(int id);
        DateTime getTableLastOrderDate(int id);
        string TimeAgo(DateTime dateTime);
        WaiterItemViewModel SelectForOrder(Item item);
    }
}
