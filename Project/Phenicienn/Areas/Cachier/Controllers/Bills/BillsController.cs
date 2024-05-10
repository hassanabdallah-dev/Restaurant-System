using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Data;

namespace Phenicienn.Areas.Cachier.Controllers.Bills
{
    [Area("Cachier")]
    public class BillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var bills = await _context.Bills.Include(m=>m.Table)
                .Where(m => m.Paid == false)
                .Where(m => m.session == m.Table.session)
                .ToListAsync();
            return View(bills);
        }
        [HttpPost]
        public void Pay(int id)
        {
            var bill = _context.Bills.Find(id);
            if(bill != null)
            {
                bill.Paid = true;
                _context.Bills.Update(bill);
                _context.SaveChanges(); 
            }
        }
    }
}