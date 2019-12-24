using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tycoon.Data;
using Tycoon.Models;
using Tycoon.Models.ViewModels;

namespace Tycoon.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext appDb)
        {
            _logger = logger;
            db = appDb;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel indexVm = new IndexViewModel()
            {
                Service = await db.Service.Include(m => m.Category)
                            .Include(m => m.SubCategory).ToListAsync(),
                Category = await db.Category.ToListAsync(),
                Coupon = await db.Coupon.Where(c => c.IsActive == true).ToListAsync()

            };

            return View(indexVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
