using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Data;
using Phenicienn.Models;
using QRCoder;

namespace Phenicienn.Areas.Admin.Controllers.tables
{
    [Area("Admin")]
    public class tablesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public tablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/tables
        public async Task<IActionResult> Index()
        {
            var user = _context.AdminUser.Find(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resto = _context.Restaurant.Find(user.Restaurant);

            var applicationDbContext = _context.tables.Where(q => q.RestaurantFK == resto.RestaurantId);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/tables/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.tables
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.id == id);
            if (table == null)
            {
                return NotFound();
            }
            ViewBag.BarcodeImage = this.getQrCode(table.key);
            return View(table);
        }

        // GET: Admin/tables/Create
        public IActionResult Create()
        {

            var user = _context.AdminUser.Find(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var resto =_context.Restaurant.Find(user.Restaurant);
            var table = _context.tables.ToList();

            
            var ids="";
            var key = "";
            if (table.Count != 0)
                ids ="/"+ resto.RestaurantId + "/" +(int)(table[table.Count()-1].id+1);
            else
                ids = "/"+resto.RestaurantId + "/" + 1;

            key = "https://localhost:44376/User/QRCodeRedirection/testKey" + ids;

            ViewBag.BarcodeImage = this.getQrCode(key);
            ViewBag.key = key;
            ViewData["RestaurantFK"] = new SelectList(_context.Restaurant, "RestaurantId", "AdminUserId");
            return View();
        }

        // POST: Admin/tables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        private string getQrCode(string key)
        {
            byte[] imageByte;
            QRCodeGenerator _qRCode = new QRCodeGenerator();
            QRCodeData _qRCodeData = _qRCode.CreateQrCode(key, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qRCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                imageByte = stream.ToArray();
            }

             return "data:image/png;base64," + Convert.ToBase64String(imageByte);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,capacity,key, TableNo, ai")] table table)
        {
            
                var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var resto = await _context.Restaurant.FindAsync(user.Restaurant);
                table.RestaurantFK = resto.RestaurantId;
                table.session = Guid.NewGuid().ToString();
                table.available = true;
            if (table.ai)
            {
                if (_context.tables.Any())
                    table.TableNo = _context.tables.Max(m => m.TableNo) + 1;
                else
                    table.TableNo = 1;
            }
               // table.key = table.RestaurantFK + "-" + table.id;
                _context.Add(table);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["RestaurantFK"] = new SelectList(_context.Restaurant, "RestaurantId", "AdminUserId", table.RestaurantFK);

            return View(table);
        }
        // GET: Admin/tables/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            ViewData["RestaurantFK"] = new SelectList(_context.Restaurant, "RestaurantId", "AdminUserId", table.RestaurantFK);
            return View(table);
        }

        // POST: Admin/tables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,capacity,available,TableNo,key")] table table)
        {
            if (id != table.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.AdminUser.FindAsync(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                    var resto = await _context.Restaurant.FindAsync(user.Restaurant);
                    table.RestaurantFK = resto.RestaurantId;
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tableExists(table.id))
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
            ViewData["RestaurantFK"] = new SelectList(_context.Restaurant, "RestaurantId", "AdminUserId", table.RestaurantFK);
            return View(table);
        }

        // GET: Admin/tables/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.tables
                .Include(t => t.Restaurant)
                .FirstOrDefaultAsync(m => m.id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Admin/tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.tables.FindAsync(id);
            _context.tables.Remove(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tableExists(int id)
        {
            return _context.tables.Any(e => e.id == id);
        }
    }
}
