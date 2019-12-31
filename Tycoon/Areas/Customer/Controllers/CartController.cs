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
using Stripe;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Tycoon.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        public readonly ApplicationDbContext db;
        private readonly IEmailSender _emailSender;
        // private OrderDetailsViewModel individualOrder;

        [BindProperty]
        public OrderDetailsCart cartDetails { get; set; }

        public CartController(ApplicationDbContext appDb, IEmailSender emailSender)
        {
            db = appDb;
            _emailSender = emailSender;
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

                if (couponFromDb != null)
                {
                    cartDetails.Order.OrderTotal =
                    StaticDetail.DiscountedPrice(couponFromDb, cartDetails.Order.OrderTotalOriginal);
                }
                else
                {
                    HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);
                }
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
                var couponFromDb = await db.Coupon.Where(c => c.Name.ToLower() == cartDetails.Order.CouponCode.ToLower()).FirstOrDefaultAsync();
                if(couponFromDb != null)
                {
                    cartDetails.Order.OrderTotal =
                    StaticDetail.DiscountedPrice(couponFromDb, cartDetails.Order.OrderTotalOriginal);
                }
                else
                {
                    HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);
                }

                
            }

            return View(cartDetails);


        }

        //Post - Summary
        [HttpPost, ActionName(name:"Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostSummary(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            cartDetails.listCart = await db.Cart.Where(c => c.UserId == claim.Value).ToListAsync();

            cartDetails.Order.PaymentStatus = StaticDetail.PaymentStatusPending;
            cartDetails.Order.OrderDate = DateTime.Now;
            cartDetails.Order.UserId = claim.Value;
            cartDetails.Order.Status = StaticDetail.PaymentStatusPending;
            cartDetails.Order.PickupTime = 
                Convert.ToDateTime(cartDetails.Order.PickupDate.ToShortDateString() + " "+
                cartDetails.Order.PickupTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            db.Order.Add(cartDetails.Order);
            await db.SaveChangesAsync();

            cartDetails.Order.OrderTotalOriginal = 0;

            foreach (var item in cartDetails.listCart)
            {
                item.Service = await db.Service.FirstOrDefaultAsync(m => m.Id == item.ServiceId);
                OrderDetails orderDetails = new OrderDetails()
                {
                    ServiceId = item.ServiceId,
                    OrderId = cartDetails.Order.Id,
                    Description = item.Service.Description,
                    ServiceName = item.Service.Name,
                    Price = item.Service.Price,
                    Count = item.Count

                };
                cartDetails.Order.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                db.OrderDetails.Add(orderDetails);
            }

            if (HttpContext.Session.GetString(StaticDetail.ssCouponCode) != null)
            {
                cartDetails.Order.CouponCode = HttpContext.Session.GetString(StaticDetail.ssCouponCode);
                var couponFromDb = await db.Coupon.Where(c => c.Name.ToLower() ==
                    cartDetails.Order.CouponCode.ToLower()).FirstOrDefaultAsync();

                    cartDetails.Order.OrderTotal =
                    StaticDetail.DiscountedPrice(couponFromDb, cartDetails.Order.OrderTotalOriginal);
                    HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);
            }
            else
            {
                cartDetails.Order.OrderTotal = cartDetails.Order.OrderTotalOriginal;
            }

            cartDetails.Order.CouponCodeDiscount = 
                cartDetails.Order.OrderTotalOriginal - cartDetails.Order.OrderTotal;
            await db.SaveChangesAsync();
            db.Cart.RemoveRange(cartDetails.listCart);
            HttpContext.Session.SetInt32(StaticDetail.ssServicesCount, 0);

            await db.SaveChangesAsync();

            var testc = cartDetails.Order.OrderTotal * 100;

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(cartDetails.Order.OrderTotal * 100),
                Currency = "cad",
                Description = "Order ID: " + cartDetails.Order.Id,
                SourceId = stripeToken

            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            if(charge.BalanceTransactionId == null)
            {
                cartDetails.Order.PaymentStatus = StaticDetail.PaymentStatusRejected;
            }
            else
            {
                cartDetails.Order.TransactionId = charge.BalanceTransactionId;
            }

            if(charge.Status.ToLower() == "succeeded")
            {
                await _emailSender
                    .SendEmailAsync(db.Users.Where(u => u.Id == claim.Value).FirstOrDefault().Email,
                    " Tycoon - Service has been booked " + cartDetails.Order.Id.ToString(),
                    " Your writing service request has been received. Please login to your account to track the order!");
                cartDetails.Order.PaymentStatus = StaticDetail.PaymentStatusApproved;
                cartDetails.Order.Status = StaticDetail.StatusSubmitted;
            }
            else
            {
                cartDetails.Order.PaymentStatus = StaticDetail.PaymentStatusRejected;
                cartDetails.Order.Status = StaticDetail.StatusCancelled;
            }

            await db.SaveChangesAsync();
            // return RedirectToAction("Index", "Home");
            return RedirectToAction("Confirm", "Order", new { id = cartDetails.Order.Id });


        }

        public IActionResult AddCoupon()
        {
           

            if (cartDetails.Order.CouponCode == null)
            {
                cartDetails.Order.CouponCode = "";
                HttpContext.Session.SetString(StaticDetail.ssCouponCode, string.Empty);
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