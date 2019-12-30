using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;
using Tycoon.Models;
using Tycoon.Models.ViewModels;
using Tycoon.Utility;

namespace Tycoon.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;
        private int PageSize = 2;

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

        [Authorize]
        public async Task<IActionResult> OrderHistory(int productPage=1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };


            List<Models.Order> orderList = await db.Order.Include(o => o.AppUser)
                .Where(u => u.UserId == claim.Value).ToListAsync();
            foreach(Models.Order item in orderList)
            {
                OrderDetailsViewModel individualOrder = new OrderDetailsViewModel()
                {
                    Order = item,
                    OrderDetails = await db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderListVM.Orders.Add(individualOrder);
            }
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders
                .OrderByDescending(p => p.Order.Id).Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();

            orderListVM.PagingInfo = new Models.PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParam = "/Customer/Order/OrderHistory?productPage=:"
            };

            return View(orderListVM);

        }

        [Authorize(Roles = StaticDetail.ManagerUser + "," + StaticDetail.WritingUser)]
        public async Task<IActionResult> ManageOrder()
        {

            List<OrderDetailsViewModel> orderDetailsVM = new List<OrderDetailsViewModel>();

            List<Models.Order> orderList = await db.Order.Include(o => o.AppUser)
                .Where(u => u.Status == StaticDetail.StatusInProgress || u.Status == StaticDetail.StatusSubmitted)
                .OrderByDescending(o => o.PickupTime).ToListAsync();
            foreach (Models.Order item in orderList)
            {
                OrderDetailsViewModel individualOrder = new OrderDetailsViewModel()
                {
                    Order = item,
                    OrderDetails = await db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderDetailsVM.Add(individualOrder);
            }

            return View(orderDetailsVM.OrderBy(o=>o.Order.PickupTime).ToList());

        }

        public async Task<IActionResult> GetOrderDetails(int id)
        {
            OrderDetailsViewModel orderdetailsVM = new OrderDetailsViewModel()
            {
                Order = await db.Order.Where(o => o.Id == id).FirstOrDefaultAsync(),
                OrderDetails = await db.OrderDetails.Where(od => od.OrderId == id).ToListAsync()
            };
            orderdetailsVM.Order.AppUser = await db.AppUser.FirstOrDefaultAsync(u => u.Id == orderdetailsVM.Order.UserId);
            return PartialView("_IndividualBookingDetails", orderdetailsVM);
        }

        public async Task<IActionResult> GetOrderStatus(int id)
        {
            OrderDetailsViewModel orderdetailsVM = new OrderDetailsViewModel()
            {
                Order = await db.Order.Where(o => o.Id == id).FirstOrDefaultAsync(),
                OrderDetails = await db.OrderDetails.Where(od => od.OrderId == id).ToListAsync()
            };
            orderdetailsVM.Order.AppUser = await db.AppUser.FirstOrDefaultAsync(u => u.Id == orderdetailsVM.Order.UserId);
            return PartialView("_OrderStatusImage", orderdetailsVM);
        }

        [Authorize(Roles = StaticDetail.WritingUser + "," + StaticDetail.ManagerUser)]
        public async Task<IActionResult> WorkOrder(int orderId)
        {
            Models.Order order = await db.Order.FindAsync(orderId);

            order.Status = StaticDetail.StatusInProgress;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));
        }

        

        
       [Authorize(Roles = StaticDetail.ManagerUser + "," + StaticDetail.WritingUser)]
        public async Task<IActionResult> OrderReady(int orderId)
        {
            Models.Order order = await db.Order.FindAsync(orderId);

            order.Status = StaticDetail.StatusReady;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));

            //Email logic to notify user
        }

        [Authorize(Roles = StaticDetail.ManagerUser + "," + StaticDetail.WritingUser)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            Models.Order order = await db.Order.FindAsync(orderId);

            order.Status = StaticDetail.StatusCancelled;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(ManageOrder));
        }

        [Authorize(Roles = StaticDetail.ManagerUser + "," + StaticDetail.CustomerSupportUser)]
        public async Task<IActionResult> OrderPickup(int productPage = 1, 
            string searchEmail=null, string searchName = null, string searchPhone = null)
        {
            /*var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);*/

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };

            StringBuilder param = new StringBuilder();
            param.Append("/Customer/Order/OrderPickup?productPage=:");
            param.Append("&searchName=");
            if (searchName != null)
            {
                param.Append(searchName);
            }
            param.Append("&searchPhone=");
            if (searchPhone != null)
            {
                param.Append(searchPhone);
            }
            param.Append("&searchEmail=");
            if (searchEmail != null)
            {
                param.Append(searchEmail);
            }

            List<Models.Order> orderList = new List<Order>();

            if (searchName !=null || searchPhone != null || searchEmail != null)
            {
                var user = new AppUser();

                if(searchName != null)
                {
                    orderList = await db.Order.Include(o=>o.AppUser)
                        .Where(o => o.PickupName.ToLower().Contains(searchName.ToLower()))
                        .OrderByDescending(o=>o.OrderDate).ToListAsync();
                }
                else
                {
                    if (searchEmail != null)
                    {
                        user = await db.AppUser.Where(u => u.Email.ToLower()
                        .Contains(searchEmail.ToLower())).FirstOrDefaultAsync();
                        orderList = await db.Order.Include(o => o.AppUser)
                            .Where(o => o.UserId == user.Id)
                            .OrderByDescending(o => o.OrderDate).ToListAsync();
                    }
                    else
                    {
                        if (searchPhone != null)
                        {
                            orderList = await db.Order.Include(o => o.AppUser)
                                .Where(o => o.PickupNumber.Contains(searchPhone))
                                .OrderByDescending(o => o.OrderDate).ToListAsync();
                        }
                    }
                }
            }
            else
            {
                orderList = await db.Order.Include(o => o.AppUser)
                .Where(u => u.Status == StaticDetail.StatusReady).ToListAsync();
            }

            foreach (Models.Order item in orderList)
            {
                    OrderDetailsViewModel individualOrder = new OrderDetailsViewModel()
                    {
                        Order = item,
                        OrderDetails = await db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                    };
                    orderListVM.Orders.Add(individualOrder);
            }

            
            
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders
                .OrderByDescending(p => p.Order.Id).Skip((productPage - 1) * PageSize)
                .Take(PageSize).ToList();

            orderListVM.PagingInfo = new Models.PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParam = param.ToString()
            };

            return View(orderListVM);

        }

        [Authorize(Roles = StaticDetail.ManagerUser + "," + StaticDetail.CustomerSupportUser)]
        [HttpPost]
        [ActionName("OrderPickup")]
        public async Task<IActionResult> OrderPickupPost(int orderId)
        {
            Models.Order order = await db.Order.FindAsync(orderId);

            order.Status = StaticDetail.StatusCompleted;

            await db.SaveChangesAsync();

            return RedirectToAction(nameof(OrderPickup));
        }

    }
}