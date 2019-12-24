using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;
using Tycoon.Models;
using Tycoon.Models.ViewModels;
using Tycoon.Utility;

namespace Tycoon.Areas.Admin.Controllers
{
    [Authorize(Roles = StaticDetail.ManagerUser)]
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext db;

        [TempData]
        public string StatusMessage { get; set; }
        public SubCategoryController(ApplicationDbContext appDb)
        {
            db = appDb;
        }

        public async Task<IActionResult> Index()
        {
            var subCategory = await db.SubCategory.
                Include(m => m.Category).ToListAsync();

            return View(subCategory);
        }

        //GET - CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = new Models.SubCategory(),
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
                .Select(s => s.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subCategoryExists = db.SubCategory.Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name &&
                    s.Category.Id == model.SubCategory.CategoryId);
                if(subCategoryExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exists under " +
                        subCategoryExists.First().Category.Name + " Category";

                }
                else
                {
                    db.SubCategory.Add(model.SubCategory);
                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVm = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
               .Select(s => s.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVm);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in db.SubCategory
                             where subCategory.CategoryId == id
                             select subCategory).ToListAsync();

            return Json(new SelectList(subCategories, "Id", "Name"));

        }

        //GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await db.SubCategory.SingleOrDefaultAsync(m=>m.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }

           
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
                .Select(s => s.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var subCategoryExists = db.SubCategory.Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name &&
                    s.Category.Id == model.SubCategory.CategoryId);
                if (subCategoryExists.Count() > 0)
                {
                    //Error
                    StatusMessage = "Error: Sub Category exists under " +
                        subCategoryExists.First().Category.Name + " Category";

                }
                else
                {
                    var subCategory = await db.SubCategory.FindAsync(model.SubCategory.Id);
                    subCategory.Name = model.SubCategory.Name;

                    await db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryViewModel modelVm = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
               .Select(s => s.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage
            };
            // modelVm.SubCategory.Id = id;
            return View(modelVm);
        }

        //GET - Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }


            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
                .Select(s => s.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //GET - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null)
            {
                return NotFound();
            }


            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await db.SubCategory.OrderBy(s => s.Name)
                .Select(s => s.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var subCategory = await db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            db.SubCategory.Remove(subCategory);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}