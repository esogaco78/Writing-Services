﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;
using Tycoon.Models;

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

        //Post - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Category.Add(category);
                await db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category);
            }

        }
    }
}