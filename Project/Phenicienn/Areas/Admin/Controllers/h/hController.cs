using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phenicienn.CustomAuthorizationAttributes;
using Phenicienn.Data;

namespace Phenicienn.Areas.Admin.Controllers.h
{
    [Area("Admin")]
    [AdminSetupAuthorize()]
    public class hController : Controller
    {
        private readonly ApplicationDbContext _context;

        public hController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}