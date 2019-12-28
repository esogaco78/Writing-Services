using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tycoon.Data;
using Tycoon.Models.ViewModels;
using Tycoon.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Tycoon.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        public readonly ApplicationDbContext db;

        [BindProperty]
        public OrderDetailsCart cartDetails { get; set; }

        public CartController(ApplicationDbContext appDb)
        {
            db = appDb;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {

            cartDetails = new OrderDetailsCart()
            {
                Order = new Models.Order()
            };

            cartDetails.Order.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = db.Cart.Where(c => c.UserId == claim.Value);

            if(cart != null)
            {
                cartDetails.listCart = cart.ToList();
            }
            foreach( var list in cartDetails.listCart)
            {
                list.Service = await db.Service.FirstOrDefaultAsync(m => m.Id == list.ServiceId);
                cartDetails.Order.OrderTotal = cartDetails.Order.OrderTotal + (list.Service.Price * list.Count);
                list.Service.Description = StaticDetail.ConvertToRawHtml(list.Service.Description);
                if(list.Service.Description.Length > 100)
                {
                    list.Service.Description = list.Service.Description.Substring(0, 99) + "...";
                }
            }
            cartDetails.Order.OrderTotalOriginal = cartDetails.Order.OrderTotal;

            return View(cartDetails);


        }

    }
}