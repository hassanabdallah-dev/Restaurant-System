using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.Services;
using Phenicienn.Areas.Waiter.ViewModels;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.Waiter.Controllers.ManageTables
{
    [Area("Waiter")]
    public class ManageTablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IViewModelFactory _viewModel;

        public ManageTablesController(ApplicationDbContext context,
                                        IViewModelFactory viewModel)
        {
            _context = context;
            _viewModel = viewModel;
        }
        public async Task<IActionResult> Index()
        {
            var resid = await _context.AdminUser.Where(m => m.Id == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).Select(m => m.Restaurant).FirstOrDefaultAsync();
            List<TableViewModel> tableViewModel = await _context.tables
                .Where(m => m.Restaurant.RestaurantId == resid).Select(
                    m => new TableViewModel
                    {
                        Id = m.id,
                        TableNo = m.TableNo,
                        capacity = m.capacity,
                        status = (m.Reservation != null)?2:((m.available)?0:1),
                        PendingOrdersCount = _viewModel.getTablePendingOrdersCount(m.id),
                        LastOrder = _viewModel.TimeAgo(_viewModel.getTableLastOrderDate(m.id)),
                        Reservation = (m.Reservation != null) ?(m.Reservation.Date.ToString("hh:mm tt")) : "",
                        Name = (m.Reservation != null) ? (m.Reservation.name) : "",
                        nb_people = (m.Reservation != null) ? (m.Reservation.nb_people) : 0,
                    }
                ).ToListAsync();
            return View(tableViewModel);
        }
        [HttpPost]
        public async Task<string> Occupy(int id){
            var table = await _context.tables.Include(m => m.Reservation).FirstOrDefaultAsync(m=>m.id == id);
            if(table == null)
                return "false";
            table.available = false;
            if (table.Reservation != null)
            {
                _context.Remove(table.Reservation);
                await _context.SaveChangesAsync();
            }
            table.session = Guid.NewGuid().ToString();
            _context.tables.Update(table);
            await _context.SaveChangesAsync();
            return "true";
        }
        [HttpPost]
        public async Task<string> Free(int id)
        {
            var table = await _context.tables.Include(m=>m.Reservation).FirstOrDefaultAsync(m => m.id == id);
            if (table == null)
                return "false";
            table.available = true;
            if(table.Reservation != null)
            {
                _context.Remove(table.Reservation);
                await _context.SaveChangesAsync();
            }
            table.session = Guid.NewGuid().ToString();
            _context.tables.Update(table);
            await _context.SaveChangesAsync();
            return "true";
        }
        [HttpPost]
        public async Task<string> Reserve(int id, DateTime reservation, int nb_people, string name)
        {
            var table = await _context.tables.FirstOrDefaultAsync(m => m.id == id);
            if (table == null)
                return "false";
            table.available = false;
            Reservation res = new Reservation() { 
                nb_people = nb_people,
                name = name,
                Date = reservation,
            };
            _context.Add(res);
            await _context.SaveChangesAsync();
            table.Reservation = res;
            table.session = Guid.NewGuid().ToString();
            _context.tables.Update(table);
            await _context.SaveChangesAsync();
            return "true";
        }
    }
}