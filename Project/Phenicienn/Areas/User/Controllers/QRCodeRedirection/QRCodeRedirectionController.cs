using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Phenicienn.Data;
using Phenicienn.Models;

namespace Phenicienn.Areas.User.Controllers.QRCodeRedirection
{
    [Area("User")]
    public class QRCodeRedirectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QRCodeRedirectionController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            if((HttpContext.Session.GetInt32("table_id")) != null)
            {
                string url = "/User/Landing/Menu/"+id;
                return Redirect(url);
            }
            ViewBag.cat = id!=null?id.ToString():"";
            return View();
        }

         
        public async Task<IActionResult> TestKey (int id, int table_id, int cat_id)

        {
            bool b;
            string ids =id + "/" + table_id;
           var key = "https://localhost:44376/User/QRCodeRedirection/testKey/" + ids;
            var tab = await _context.tables.FirstOrDefaultAsync(m => m.key == key);
            b = tab == null?false:true;

            if (b == true)
            {
                HttpContext.Session.SetInt32("table_id", table_id);
                HttpContext.Session.SetInt32("res_id", id);
                string url = "/User/Landing/Menu/";
                tab.available = false;
                _context.tables.Update(tab);
                await _context.SaveChangesAsync();
                if (cat_id != 0)
                    url += cat_id;
                return Redirect(url);
            }
            else
            {
                return Redirect("/");              
            }
        }
    }

   
}