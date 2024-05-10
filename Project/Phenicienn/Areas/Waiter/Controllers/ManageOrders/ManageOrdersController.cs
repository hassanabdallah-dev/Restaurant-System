using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Areas.Waiter.ViewModels;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.Waiter.Controllers.ManageOrders
{
    [Area("Waiter")]

    public class ManageOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IViewModelFactory _viewModel;

        public ManageOrdersController(ApplicationDbContext context,
                                        IViewModelFactory viewModel)
        {
            _context = context;
            _viewModel = viewModel;
        }
        public async Task<IActionResult> Index(string sessionId = null)
        {
            var PendingOrders = await GetOrders(0, sessionId);
            return View(PendingOrders);
        }
        [HttpPost]
        public async Task<JsonResult> Validate(int OId)
        {
            var order = await _context.Orders.Include(m => m.table).FirstOrDefaultAsync(m => m.Order_Id ==  OId);
            if (order == null)
                return Json(new
                {
                    message = "fail"
                });
            var id = order.table.id;
            order.Status = 1;
            _context.Update(order);
            await _context.SaveChangesAsync();
            var pendingg = _viewModel.getTablePendingOrdersCount(id);
            var pendingstring = pendingg == 0? "No Pending Orders" : pendingg + " Pending Orders";
            var lastOrder = _viewModel.TimeAgo(_viewModel.getTableLastOrderDate(id));
            lastOrder = lastOrder != "" ? "Last Order " + lastOrder :"Last Order N/A";
            return Json(new { 
                message="success",
                pending=pendingstring,
                LastOrder = lastOrder
            });
        }
        [HttpPost]
        public async Task<JsonResult> Cancel(int OId)
        {
            var order = await _context.Orders.Include(m => m.table).FirstOrDefaultAsync(m => m.Order_Id == OId);
            if (order == null)
                return Json(new
                {
                    message = "fail"
                });
            var id = order.table.id;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            var pendingg = _viewModel.getTablePendingOrdersCount(id);
            var pendingstring = pendingg == 0 ? "No Pending Orders" : pendingg + " Pending Orders";
            var lastOrder = _viewModel.TimeAgo(_viewModel.getTableLastOrderDate(id));
            lastOrder = lastOrder != "" ? "Last Order " + lastOrder : "Last Order N/A";
            return Json(new {
                message = "success",
                pending = pendingstring,
                LastOrder = lastOrder
            });
        }

        public async Task<List<WaiterOrderViewModel>> GetOrders(int status, string sessionId = null)
        {
            var order = _context.Orders
                .Include(m => m.OrdersItems)
                .ThenInclude(m => m.Item)
                .Where(m => m.Status == status)
                .Select(m => new WaiterOrderViewModel
                {
                    Order_Id = m.Order_Id,
                    Date = m.Date,
                    HumanDate = _viewModel.TimeAgo(m.Date),
                    session = m.session,
                    OrdersItems = m.OrdersItems.Select(m => new WaiterOrdersItemsViewModel
                    {
                        Quantity = m.Quantity,
                        Id = m.Id,
                        Item = _viewModel.SelectForOrder(m.Item)
                    }).ToList()
                }).OrderBy(m => m.Date);
            if (order == null)
                return null;
            if (sessionId == null)
                return await order.ToListAsync();
            else
                return await order.Where(m => m.session == sessionId).ToListAsync();
        }
        public async Task<JsonResult> getOrdersJsonByTable(int id)
        {
            var session = await _context.tables.Where(m => m.id == id).Select(m => m.session).FirstOrDefaultAsync();
            if (session == null)
                return Json("");
            return await getOrdersJson(0, session);
        }
        public async Task<JsonResult> getOrdersJson(int status, string sessionId = null)
        {
            return Json(await this.GetOrders(status, sessionId), new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        }
    }
}