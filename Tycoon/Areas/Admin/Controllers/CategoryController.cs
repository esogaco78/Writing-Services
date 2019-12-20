using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;

namespace Tycoon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoryController(ApplicationDbContext appDb)
        {
            db = appDb;
        }

        //GET Action method
        public async Task<IActionResult> Index()
        {
            return View(await db.Category.ToListAsync());
        }

        //GET - Create
        public IActionResult Create()
        {
            return View();
        }
    }
}