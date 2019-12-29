using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;
using Tycoon.Models.ViewModels;

namespace Tycoon.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;

        public OrderController(ApplicationDbContext appDb)
        {
            db = appDb;
        }

        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel orderDetailsVM = new OrderDetailsViewModel()
            {
                Order = await db.Order.Include(o => o.AppUser)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = await db.OrderDetails.Where(od => od.OrderId == id).ToListAsync()

            };

            return View(orderDetailsVM);

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}