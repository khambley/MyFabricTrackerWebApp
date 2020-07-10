using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFabricTrackerWebApp.Models;

namespace MyFabricTrackerWebApp.Controllers
{
    public class FabricsController : Controller
    {
        private readonly FabricTrackerDbContext _context;

        public FabricsController(FabricTrackerDbContext context)
        {
            _context = context;
        }

        // GET: Fabrics
        public async Task<IActionResult> Index()
        {
            var fabricTrackerDbContext = _context.Fabrics.Include(f => f.MainCategory).Include(f => f.SubCategory);
            return View(await fabricTrackerDbContext.ToListAsync());
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
            List<MainCategory> mainCategoriesList = _context.MainCategories.ToList();
            mainCategoriesList.Insert(0, new MainCategory { MainCategoryId = 0, MainCategoryName = "--Select Main Category--" });
            
            ViewData["MainCategoryId"] = new SelectList(mainCategoriesList, "MainCategoryId", "MainCategoryName");
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName");
            return View();
        }

        // POST: Fabrics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricID,FabricItemCode,FabricName,ImageFileName,MainCategoryId,SubCategoryId,FabricType,FabricNotes,FabricSourceName,FabricSourceUrl,DateAdded,DateModified,IsDeleted")] Fabric fabric)
        {
            fabric.DateAdded = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(fabric);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MainCategoryId"] = new SelectList(_context.MainCategories, "MainCategoryId", "MainCategoryName", fabric.MainCategoryId);
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "SubCategoryName", fabric.SubCategoryId);
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
    }
}
