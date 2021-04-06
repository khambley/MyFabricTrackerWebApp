using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFabricTrackerWebApp.Models;

namespace MyFabricTrackerWebApp.Controllers
{
    public class FabricsController : Controller
    {
        private readonly FabricTrackerDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //This is a comment to test github account.

        public FabricsController(FabricTrackerDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Fabrics
        public async Task<IActionResult> Index()
        {
            List<Fabric> fabricList = new List<Fabric>();
            fabricList = await _context.Fabrics
                .Include(f => f.MainCategory)
                .Include(f => f.SubCategory)
                .Include(f => f.Source)
                .Include(f => f.FabricType)
                .ToListAsync();
            
            //var fabricTrackerDbContext = _context.Fabrics.Include(f => f.MainCategory).Include(f => f.SubCategory);
            
            return View(fabricList);
        }

        // GET: Fabrics/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
                .Include(f => f.MainCategory)
                .Include(f => f.SubCategory)
                .Include(f => f.Source)
                .Include(f => f.FabricType)
                .FirstOrDefaultAsync(m => m.FabricID == id);
            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }

        // GET: Fabrics/Create
        public IActionResult Create()
        {
            List<MainCategory> mainCategoryList = new List<MainCategory>();

            // -- Create main categories dropdown
            mainCategoryList = _context.MainCategories
                        .OrderBy(mc => mc.MainCategoryName).ToList();
            mainCategoryList.Insert(0, new MainCategory { MainCategoryId = 0, MainCategoryName = "Select" });
            ViewBag.MainCategoryList = mainCategoryList;

            // -- Create fabric types dropdown
            long fabricTypeId = 0;
            ViewData["FabricTypeId"] = new SelectList(_context.FabricTypes.OrderByDescending(ft => ft.Name), "Id", "Name", fabricTypeId);

            // Create sources dropdown and add sources to it.
            long sourceId = 0;
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", sourceId);
            
            return View();
        }
        public JsonResult GetSubCategoryList(int MainCategoryId)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            // ------ Getting Data from Database Using EF Core ------
            subCategoryList = (from subcategory in _context.SubCategories
                               where subcategory.MainCategoryId == MainCategoryId
                               orderby subcategory.SubCategoryName ascending
                               select subcategory).ToList();

            // ------ Inserting SubCategory Select Items into List
            subCategoryList.Insert(0, new SubCategory { SubCategoryId = 0, SubCategoryName = "Select" });

            return Json(new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName"));
        }
        // POST: Fabrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,FabricType,FabricWidth,BackgroundColor,FabricNotes,AccentColor1,AccentColor2,SourceId,FabricTypeId,AccentColor3,FabricSourceUrl,DateAdded,DateModified,IsDiscontinued,IsDeleted,IsPopular")] Fabric fabric, IFormFile imageFile)
        {
            //Set the auto-generated Item Code 
            fabric.FabricItemCode = CreateUniqueItemCode();

            if (imageFile != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                //Set the Image File Name (generated using FabricItemCode) to prevent overwriting files with same name
                var uniqueFileName = fabric.FabricItemCode + Path.GetExtension(imageFile.FileName);
                
                //Set the Image File Path.
                var filePath = Path.Combine(webRootPath + "\\images\\", uniqueFileName);

                //Save the Image File to folder on file system.
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                fabric.ImageFileName = uniqueFileName;
            }

            fabric.DateAdded = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(fabric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "MainCategoryId", "MainCategoryName", fabric.MainCategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", fabric.SourceId);
            ViewData["FabricTypeId"] = new SelectList(_context.FabricTypes.OrderByDescending(ft => ft.Name), "FabricTypeId", "Name", fabric.FabricTypeId);
            return View(fabric);
        }

        // GET: Fabrics/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics.FindAsync(id);
            if (fabric == null)
            {
                return NotFound();
            }

            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "MainCategoryId", "MainCategoryName", fabric.MainCategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", fabric.SourceId);

            return View(fabric);
        }

        // POST: Fabrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,FabricType,FabricNotes,FabricSourceName,FabricSourceUrl,DateAdded,DateModified,IsDeleted")] Fabric fabric)
        {
            fabric.DateAdded = DateTime.Now;
            if (id != fabric.FabricID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabric);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricExists(fabric.FabricID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "MainCategoryId", "MainCategoryName", fabric.MainCategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId);
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", fabric.SourceId);
            return View(fabric);
        }

        // GET: Fabrics/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _context.Fabrics
                .Include(f => f.MainCategory)
                .Include(f => f.SubCategory)
                .Include(f => f.Source)
                .FirstOrDefaultAsync(m => m.FabricID == id);
            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }

        // POST: Fabrics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var fabric = await _context.Fabrics.FindAsync(id);
            _context.Fabrics.Remove(fabric);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricExists(long id)
        {
            return _context.Fabrics.Any(e => e.FabricID == id);
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_"
                + Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }
        private string CreateUniqueItemCode()
        {
            var lastFabricinDB = _context.Fabrics
                .OrderByDescending(f => f.FabricID)
                .FirstOrDefault();
           
            var newFabricId = lastFabricinDB != null ? lastFabricinDB.FabricID + 1 : 1;

            string leadingZeroes = "";

            if (newFabricId > 0 && newFabricId < 10)
            {
                leadingZeroes = "0000";
            }
            else if (newFabricId > 9 && newFabricId < 100)
            {
                leadingZeroes = "000";
            }
            else if (newFabricId > 99 && newFabricId < 1000)
            {
                leadingZeroes = "00";
            }
            else if (newFabricId > 999 && newFabricId < 10000)
            {
                leadingZeroes = "0";
            }
            else
            {
                leadingZeroes = "";
            }
            var uniqueFabricItemCode = "FAB" 
                + "-"
                + leadingZeroes
                + newFabricId.ToString()
                + "-"
                + Guid.NewGuid().ToString().ToUpper().Substring(0, 4);

            return uniqueFabricItemCode;
        }
    }
}
