using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tycoon.Data;
using Tycoon.Models.ViewModels;
using Tycoon.Utility;

namespace Tycoon.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext db;

        //For uploading images
        private readonly IWebHostEnvironment hostingEnvironment;

        //This avoids passing model as parameter in all the methods like in Category and SubCat
        [BindProperty]
        public ServiceViewModel ServiceVM { get; set; }
        public ServiceController(ApplicationDbContext appDb, IWebHostEnvironment hostingEnv )
        {
            db = appDb;
            hostingEnvironment = hostingEnv;
            ServiceVM = new ServiceViewModel()
            {
                Category = db.Category,
                Service = new Models.Service()
            };
        }

        public async Task<IActionResult> Index()
        {
            var services = await db.Service
                .Include(m=> m.Category).Include(m=>m.SubCategory).ToListAsync();
            return View(services);
        }
        //GET -CREATE
        public IActionResult Create()
        {
            return View(ServiceVM);
        }

       
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            ServiceVM.Service.SubCategoryId = Convert.ToInt32
                (Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(ServiceVM);
            }
            db.Service.Add(ServiceVM.Service);
            await db.SaveChangesAsync();

            //Image saving
            string webRootPath = hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var serviceFromDb = await db.Service.FindAsync(ServiceVM.Service.Id);

            if(files.Count() > 0)
            {
                //file has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStrem = new FileStream
                    (Path.Combine(uploads, ServiceVM.Service.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStrem);
                }
                serviceFromDb.Image = @"\images\" + ServiceVM.Service.Id + extension;
            }
            else
            {
                //No file uploaded, so use default
                var uploads = Path.Combine(webRootPath, @"images\"+ StaticDetail.DeafautServiceImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + ServiceVM.Service.Id + ".png");
                serviceFromDb.Image = @"\images\" + ServiceVM.Service.Id + ".png";
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET -Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ServiceVM.Service = await db.Service.Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            ServiceVM.SubCategory = await db.SubCategory
                .Where(s => s.CategoryId == ServiceVM.Service.CategoryId).ToListAsync();
            if(ServiceVM.Service == null)
            {
                return NotFound();
            }
            return View(ServiceVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            ServiceVM.Service.SubCategoryId = Convert.ToInt32
                (Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                ServiceVM.SubCategory = await db.SubCategory
                    .Where(s => s.CategoryId == ServiceVM.Service.CategoryId).ToListAsync();
                return View(ServiceVM);
            }
            
            //Image saving
            string webRootPath = hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var serviceFromDb = await db.Service.FindAsync(ServiceVM.Service.Id);

            if (files.Count() > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original file

                var imagePath = Path.Combine(webRootPath, serviceFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                //Upload new file
                using (var fileStrem = new FileStream
                    (Path.Combine(uploads, ServiceVM.Service.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(fileStrem);
                }
                serviceFromDb.Image = @"\images\" + ServiceVM.Service.Id + extension_new;
            }

            serviceFromDb.Name = ServiceVM.Service.Name;
            serviceFromDb.Description = ServiceVM.Service.Description;
            serviceFromDb.Price = ServiceVM.Service.Price;
            serviceFromDb.CategoryId = ServiceVM.Service.CategoryId;
            serviceFromDb.SubCategoryId = ServiceVM.Service.CategoryId;
            serviceFromDb.Popularity = ServiceVM.Service.Popularity;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //GET -Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ServiceVM.Service = await db.Service.Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            ServiceVM.SubCategory = await db.SubCategory
                .Where(s => s.CategoryId == ServiceVM.Service.CategoryId).ToListAsync();
            if (ServiceVM.Service == null)
            {
                return NotFound();
            }
            return View(ServiceVM);
        }

        //GET -Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ServiceVM.Service = await db.Service.Include(m => m.Category)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            ServiceVM.SubCategory = await db.SubCategory
                .Where(s => s.CategoryId == ServiceVM.Service.CategoryId).ToListAsync();
            if (ServiceVM.Service == null)
            {
                return NotFound();
            }
            return View(ServiceVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var serviceFromDb = await db.Service.FindAsync(id);
            if (serviceFromDb == null)
            {
                return NotFound();
            }

            string webRootPath = hostingEnvironment.WebRootPath;
          
            //Delete the original file

            var imagePath = Path.Combine(webRootPath, serviceFromDb.Image.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            db.Service.Remove(serviceFromDb);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}