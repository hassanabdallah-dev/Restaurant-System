using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.Admin.Controllers.Allergants
{
    [Area("Admin")]
    public class AllergantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AllergantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Allergants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Allergants.ToListAsync());
        }

        // GET: Admin/Allergants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergant = await _context.Allergants
                .FirstOrDefaultAsync(m => m.AllergantId == id);
            if (allergant == null)
            {
                return NotFound();
            }

            return View(allergant);
        }

        // GET: Admin/Allergants/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Allergants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllergantId,Name,Description")] Allergant allergant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(allergant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(allergant);
        }

        // GET: Admin/Allergants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergant = await _context.Allergants.FindAsync(id);
            if (allergant == null)
            {
                return NotFound();
            }
            return View(allergant);
        }

        // POST: Admin/Allergants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AllergantId,Name,Description")] Allergant allergant)
        {
            if (id != allergant.AllergantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allergant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllergantExists(allergant.AllergantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(allergant);
        }

        // GET: Admin/Allergants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var allergant = await _context.Allergants
                .FirstOrDefaultAsync(m => m.AllergantId == id);
            if (allergant == null)
            {
                return NotFound();
            }

            return View(allergant);
        }

        // POST: Admin/Allergants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var allergant = await _context.Allergants.FindAsync(id);
            _context.Allergants.Remove(allergant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AllergantExists(int id)
        {
            return _context.Allergants.Any(e => e.AllergantId == id);
        }
    }
}
