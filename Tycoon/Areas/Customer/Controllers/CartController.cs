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
using Microsoft.AspNetCore.Http;
using Tycoon.Models;

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

            if(HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                cartDetails.Order.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await
                    db.Coupon
                    .Where(c => c.Name.ToLower() == 
                    cartDetails.Order.CouponCode.ToLower()).FirstOrDefaultAsync();

                cartDetails.Order.OrderTotal = 
                    StaticDetail.DiscountedPrice(couponFromDb, cartDetails.Order.OrderTotal);
            }

            return View(cartDetails);


        }

        public async Task<IActionResult> Summary()
        {

            cartDetails = new OrderDetailsCart()
            {
                Order = new Models.Order()
            };

            cartDetails.Order.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            AppUser appUser = await db.AppUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();

            var cart = db.Cart.Where(c => c.UserId == claim.Value);

            if (cart != null)
            {
                cartDetails.listCart = cart.ToList();
            }
            foreach (var list in cartDetails.listCart)
            {
                list.Service = await db.Service.FirstOrDefaultAsync(m => m.Id == list.ServiceId);
                cartDetails.Order.OrderTotal = cartDetails.Order.OrderTotal + (list.Service.Price * list.Count);
                list.Service.Description = StaticDetail.ConvertToRawHtml(list.Service.Description);
            }
            cartDetails.Order.OrderTotalOriginal = cartDetails.Order.OrderTotal;
            cartDetails.Order.PickupName = appUser.FirstName;
            cartDetails.Order.PickupNumber = appUser.PhoneNumber;
            cartDetails.Order.PickupTime = DateTime.Now;

            if (HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                cartDetails.Order.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await
                    db.Coupon
                    .Where(c => c.Name.ToLower() ==
                    cartDetails.Order.CouponCode.ToLower()).FirstOrDefaultAsync();

                cartDetails.Order.OrderTotal =
                    StaticDetail.DiscountedPrice(couponFromDb, cartDetails.Order.OrderTotal);
            }

            return View(cartDetails);


        }

        public IActionResult AddCoupon()
        {
            if(cartDetails.Order.CouponCode == null)
            {
                cartDetails.Order.CouponCode = "";
            }
            HttpContext.Session.SetString(StaticDetail.ssCouponCode, cartDetails.Order.CouponCode);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Plus (int cartId)
        {
            var cart = await db.Cart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await db.Cart.FirstOrDefaultAsync(c => c.Id == cartId);
            if(cart.Count == 1)
            {
                db.Cart.Remove(cart);
                await db.SaveChangesAsync();

                var cnt = db.Cart.Where(u => u.UserId == cart.UserId).ToList().Count;
                HttpContext.Session.SetInt32(StaticDetail.ssServicesCount, cnt);

            }
            else
            {
                cart.Count -= 1;
                await db.SaveChangesAsync();
            }  
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await db.Cart.FirstOrDefaultAsync(c => c.Id == cartId);

            db.Cart.Remove(cart);
            await db.SaveChangesAsync();

            var cnt = db.Cart.Where(u => u.UserId == cart.UserId).ToList().Count;
            HttpContext.Session.SetInt32(StaticDetail.ssServicesCount, cnt);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}