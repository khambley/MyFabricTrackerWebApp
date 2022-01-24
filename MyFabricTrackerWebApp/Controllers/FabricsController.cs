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
using Microsoft.Extensions.FileProviders;
using MyFabricTrackerWebApp.Helpers;
using MyFabricTrackerWebApp.Models;
using MyFabricTrackerWebApp.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace MyFabricTrackerWebApp.Controllers
{
    public class FabricsController : Controller
    {
        private readonly IFabricService _fabricService;
        private readonly FabricTrackerDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FabricsController(IFabricService fabricService, FabricTrackerDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _fabricService = fabricService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        #region Fabric List (Index)
        // GET: Fabrics
        public async Task<IActionResult> Index(
            string sortOrder, 
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            // Get all fabrics
            var fabrics = _fabricService.GetFabrics();

            // Get Fabric count
            ViewBag.TotalFabricCount = fabrics.Count();

            // Set page number
            if (searchString != null)
			{
                pageNumber = 1;
			}
            else
			{
                searchString = currentFilter;
			}
            
            ViewData["CurrentFilter"] = searchString;

            // Get Fabrics By search term
            if (!String.IsNullOrEmpty(searchString))
			{
                fabrics = _fabricService.GetFabricsBySearchTerm(fabrics, searchString);
			}

            ViewData["CurrentSort"] = sortOrder;

            // Sort Fabrics by Name and Date
            fabrics = _fabricService.SortFabrics(fabrics, sortOrder);

            int pageSize = 12;
            
            return View(await PaginatedList<Fabric>.CreateAsync(fabrics.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        #endregion
        #region Fabric Detail
        
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabric = await _fabricService.GetFabricById(id);

            if (fabric == null)
            {
                return NotFound();
            }

            return View(fabric);
        }
        #endregion

        #region GET: Fabric Create Page (Create)
        // GET: Fabrics/Create
        public IActionResult Create()
        {
            
            ViewBag.MainCategoryList = CreateMainCategoriesList();
            
            // -- Create fabric types dropdown
            long fabricTypeId = 0;
            ViewData["FabricTypeId"] = new SelectList(_context.FabricTypes.OrderByDescending(ft => ft.Name), "Id", "Name", fabricTypeId);

            // Create sources dropdown and add sources to it.
            long sourceId = 0;
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", sourceId);
            
            return View();
        }
		#endregion
        public List<MainCategory> CreateMainCategoriesList()
		{
            List<MainCategory> mainCategoryList = new List<MainCategory>();

            // -- Create main categories dropdown
            mainCategoryList = _context.MainCategories
                        .OrderBy(mc => mc.MainCategoryName).ToList();
            mainCategoryList.Insert(0, new MainCategory { MainCategoryId = 0, MainCategoryName = "Select" });

            return mainCategoryList;
        }
		public JsonResult GetSubCategoryList(int MainCategoryId, Fabric fabric = null)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            // ------ Getting Data from Database Using EF Core ------
            subCategoryList = (from subcategory in _context.SubCategories
                               where subcategory.MainCategoryId == MainCategoryId
                               orderby subcategory.SubCategoryName ascending
                               select subcategory).ToList();

            // ------ Inserting SubCategory Select Items into List
            subCategoryList.Insert(0, new SubCategory { SubCategoryId = 0, SubCategoryName = "Select" });
            if (fabric != null)
            {
                return Json(new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId));
            } else
			{
                return Json(new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName"));
            }
            
        }

        public JsonResult GetSubCategoryListEdit(int MainCategoryId, Fabric fabric = null)
        {
            List<SubCategory> subCategoryList = new List<SubCategory>();

            // ------ Getting Data from Database Using EF Core ------
            subCategoryList = (from subcategory in _context.SubCategories
                               where subcategory.MainCategoryId == MainCategoryId
                               orderby subcategory.SubCategoryName ascending
                               select subcategory).ToList();

            // ------ Inserting SubCategory Select Items into List
            subCategoryList.Insert(0, new SubCategory { SubCategoryId = 0, SubCategoryName = "Select" });
            if(fabric != null)
			{
                return Json(new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId));
            }
            return Json(new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName"));
        }
        public List<SubCategory> CreateSubCategoriesList(long mainCategoryId)
		{
            List<SubCategory> subCategoryList = new List<SubCategory>();

            subCategoryList = (from subcategory in _context.SubCategories
                                where subcategory.MainCategoryId == mainCategoryId
                                orderby subcategory.SubCategoryName ascending
                                select subcategory).ToList();

            return subCategoryList;
        }
        #region POST: Fabric Create Page (Create)
        // POST: Fabrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,FabricType,FabricWidth,BackgroundColor,FabricNotes,AccentColor1,AccentColor2,SourceId,FabricTypeId,AccentColor3,FabricSourceUrl,DateReleased,DateAdded,DateModified,IsDiscontinued,IsDeleted,IsPopular,TotalInches,FatQtrQty")] Fabric fabric, IFormFile imageFile)
        {
            //Set the auto-generated Item Code 
            fabric.FabricItemCode = _fabricService.CreateUniqueItemCode();

			try
			{
                if(imageFile != null)
				{
                    string webRootPath = _webHostEnvironment.WebRootPath;

                    //Set the Image File Name (generated using FabricItemCode) to prevent overwriting files with same name
                    var uniqueFileName = fabric.FabricItemCode + Path.GetExtension(imageFile.FileName);

                    using (var image = Image.Load(imageFile.OpenReadStream()))
                    {
                        int width = image.Width / 2;
                        int height = image.Height / 2;

                        image.Mutate(x => x.Resize(width, height));
                        var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{uniqueFileName}";

                        image.Save(filePath);
                    }

                    fabric.ImageFileName = uniqueFileName;
                }   
            }
            catch (Exception ex)
			{
                Console.WriteLine(ex.Message);
                throw;
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
        #endregion
        
        #region GET: Edit Fabric Page
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

            if(fabric.TotalInches == null)
			{
                ViewBag.TotalInches = 0.00;
			} 
            else
			{
                ViewBag.TotalInches = fabric.TotalInches;

            }
            ViewBag.FabricItemCode = fabric.FabricItemCode;
            ViewBag.TotalFatQtrs = fabric.FatQtrQty;

            ViewBag.MainCategoryList = CreateMainCategoriesList();

            ViewBag.SubCategoryName = _context.SubCategories.Where(sc => sc.SubCategoryId == fabric.SubCategoryId).Select(name => name.SubCategoryName).FirstOrDefault();

            ViewBag.MainCategory = _context.MainCategories.Where(mc => mc.MainCategoryId == fabric.MainCategoryId).Select(name => name.MainCategoryName).FirstOrDefault();

            long subCategoryId = fabric.SubCategoryId;

            ViewBag.ImageFileName = fabric.ImageFileName;
            // ------ Getting Data from Database Using EF Core ------

            ViewBag.SubCategoryList = CreateSubCategoriesList(fabric.MainCategoryId);
            //ViewData["SubCategoryId"] = new SelectList(subCategoryList, "SubCategoryId", "SubCategoryName", subCategoryId);

            // -- Create fabric types dropdown
            int? fabricTypeId = fabric.FabricTypeId;
            ViewData["FabricTypeId"] = new SelectList(_context.FabricTypes.OrderByDescending(ft => ft.Name), "Id", "Name", fabricTypeId);

            // Create sources dropdown and add sources to it.
            int? sourceId = fabric.SourceId;
            ViewData["SourceId"] = new SelectList(_context.Sources, "SourceId", "SourceName", sourceId);

            return View(fabric);
        }
        #endregion

        // POST: Fabrics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,FabricType,FabricNotes,FabricSourceName,FabricSourceUrl,DateReleased,DateAdded,DateModified,IsDeleted")] 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,TotalInches,FatQtrQty,FabricTypeId,Brand,Designer,FabricWidth,FabricNotes,SourceId,FabricSourceName,FabricSourceUrl,DateReleased,DateAdded,DateModified,IsDeleted,IsPopular,BackgroundColor,AccentColor1,AccentColor2,AccentColor3")] Fabric fabric, IFormFile imageFile)
        {
            fabric.DateModified = DateTime.Now;

            if (id != fabric.FabricID)
            {
                return NotFound();
            }
            var existingSubCategoryId = _context.Fabrics.Where(f => f.FabricID == id).Select(sc => sc.SubCategoryId).FirstOrDefault();

            if(fabric.SubCategoryId == 0 && existingSubCategoryId > 0)
			{
                fabric.SubCategoryId = existingSubCategoryId;
			}

            var existingImageFileName = _context.Fabrics.Where(f => f.FabricID == id).Select(fn => fn.ImageFileName).FirstOrDefault();
            
            if (existingImageFileName != null && imageFile == null)
            {
                fabric.ImageFileName = existingImageFileName;
            }
            else
			{
                try
                {
                    if (imageFile != null)
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Set the Image File Name (generated using FabricItemCode) to prevent overwriting files with same name
                        var uniqueFileName = fabric.FabricItemCode + Path.GetExtension(imageFile.FileName);

                        using (var image = Image.Load(imageFile.OpenReadStream()))
                        {
                            int width = image.Width / 2;
                            int height = image.Height / 2;

                            image.Mutate(x => x.Resize(width, height));
                            var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{uniqueFileName}";

                            image.Save(filePath);
                        }

                        fabric.ImageFileName = uniqueFileName;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
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

            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "MainCategoryId", "MainCategoryName");
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
                .Include(f => f.FabricType)
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
        
    }
}
