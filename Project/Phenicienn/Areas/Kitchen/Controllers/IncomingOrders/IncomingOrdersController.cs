using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Kitchen.ViewModels;
using Phenicienn.Data;

namespace Phenicienn.Areas.Kitchen.Controllers.IncomingOrders
{
    [Area("Kitchen")]
    public class IncomingOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncomingOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userid = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var resid = _context.AdminUser.Find(userid).Restaurant;
            var orders = await _context.Orders
                .Include(m => m.table)
                .Include(m => m.OrdersItems)
                .ThenInclude(m => m.Item)
                .Where(m => m.Status == 1)
                .Where(m => m.table.RestaurantFK == resid)
                .Select(m => new KitchenOrderViewModel { 
                    Id = m.Order_Id,
                    TableNo = m.table.TableNo,
                    Comment = m.Commment,
                    items = m.OrdersItems.Select(m => new KitchenItemViewModel
                    {
                        Name = m.Item.Name,
                        ImagePath = m.Item.ImagePath
                    }).ToList()
                })
                .ToListAsync();

            return View(orders);
        }
        [HttpPost]
        public void Ready(int id)
        {
            var order = _context.Orders.Find(id);
            if(order != null)
            {
                order.Status = 2;
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
        }
        [HttpPost]
        public void Remove(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }
}