using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Phenicienn.Areas.Admin.BindingModels;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.Admin.Controllers.Categories
{
    [Area("Admin")]
    [AdminSetupAuthorize()]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoriesController(ApplicationDbContext context,
                                        IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            webHostEnvironment = hostEnvironment;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.Where(m=>m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value).OrderBy(m => m.priority).ToListAsync());
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Where(m => m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }
        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Image")] EditCategory cat)
        {
            
            if (ModelState.IsValid)
            {
                if (await _context.Category.AnyAsync(m => m.Name == cat.Name))
                    return View(cat);
                Category category = new Category()
                {
                    Name = cat.Name,
                    Image = cat.Image,
                    owner = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
                };
                category.ImagePath = UploadedFile(category);
                int maxp = 0;
                if (await _context.Category.AnyAsync(m => m.owner == category.owner))
                {
                    maxp = await _context.Category
                    .Where(m => m.owner == category.owner)
                    .Select(m => m.priority)
                    .MaxAsync();
                }
                category.priority = maxp + 1;
                _context.Add(category);
                await _context.SaveChangesAsync();
                _context.Add(new CategoryFilter() { Category = category});
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Categories", new { area = "Admin" });
            }
            return View(cat);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Where(m => m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Image")] EditCategory cat)
        {
            Category category = null;
            if (id != cat.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category = await _context.Category
                        .Where(m => m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                        .FirstOrDefaultAsync(m => m.CategoryId == id);

                    if(cat.Image != null)
                    {
                        category.Image = cat.Image;
                        category.ImagePath = UploadedFile(category);
                    }
                    category.Name = cat.Name;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Categories", new { area = "Admin" });
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                        .Where(m => m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                        .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category
                        .Where(m => m.owner == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value)
                        .FirstOrDefaultAsync(m => m.CategoryId == id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Categories", new { area = "Admin" });
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
        private string UploadedFile(Category category)
        {
            string uniqueFileName = null, filePath = null;

            if (category.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + category.Image.FileName;
                filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    category.Image.CopyTo(fileStream);
                }
            }
            return  "images/"+uniqueFileName;
        }
    }
}
