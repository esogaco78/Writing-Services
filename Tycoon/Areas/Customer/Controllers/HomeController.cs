using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tycoon.Data;
using Tycoon.Models;
using Tycoon.Models.ViewModels;
using Tycoon.Utility;

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

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                var currentCount = db.Cart
                    .Where(c => c.UserId == claim.Value).ToList().Count;

                HttpContext.Session.SetInt32(StaticDetail.ssServicesCount, currentCount);
            }

            return View(indexVm);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var serviceFromDB = await db.Service.Include(m => m.Category)
                .Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();
            Cart cartObject = new Cart()
            {
                Service = serviceFromDB,
                ServiceId = serviceFromDB.Id

            };

            return View(cartObject);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(Cart CartObj)
        {
            CartObj.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObj.UserId = claim.Value;

                Cart cartFromDb = await db.Cart.Where(c => c.UserId == CartObj.UserId
                                                && c.ServiceId == CartObj.ServiceId).FirstOrDefaultAsync();
                if(cartFromDb == null)
                {
                    await db.Cart.AddAsync(CartObj);
                }
                else
                {
                    cartFromDb.Count += CartObj.Count;
                }

                await db.SaveChangesAsync();

                var cnt = db.Cart.Where(c => c.UserId == CartObj.UserId).ToList().Count();

                HttpContext.Session.SetInt32(StaticDetail.ssServicesCount, cnt);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var serviceFromDb = await db.Service.Include(m => m.Category)
                .Include(m => m.SubCategory).Where(m => m.Id == CartObj.ServiceId).FirstOrDefaultAsync();

                Cart cartObject = new Cart()
                {
                    Service = serviceFromDb,
                    ServiceId = serviceFromDb.Id
                };

                return View(cartObject);
            }

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
