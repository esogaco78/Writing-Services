using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tycoon.Data;
using Microsoft.EntityFrameworkCore;

namespace Tycoon.ViewComponents
{
    public class UserNameViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext db;

        public UserNameViewComponent(ApplicationDbContext Appdb)
        {
            db = Appdb;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var userFromDb = await db.AppUser.FirstOrDefaultAsync(u => u.Id == claim.Value);

            return View(userFromDb);
        }
    }
}
