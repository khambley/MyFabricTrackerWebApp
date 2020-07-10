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
    public class MainCategoriesController : Controller
    {
        private readonly FabricTrackerDbContext _context;

        public MainCategoriesController(FabricTrackerDbContext context)
        {
            _context = context;
        }

        // GET: MainCategories
        public async Task<IActionResult> Index()
        {
            var orderedList = _context.MainCategories.Include(c => c.SubCategories)
                .OrderBy(n => n.MainCategoryName).ToListAsync();
            return View(await orderedList);
        }

        // GET: MainCategories/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategories
                .FirstOrDefaultAsync(m => m.MainCategoryId == id);
            if (mainCategory == null)
            {
                return NotFound();
            }

            return View(mainCategory);
        }

        // GET: MainCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MainCategoryId,MainCategoryName,IsDeleted")] MainCategory mainCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mainCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mainCategory);
        }

        // GET: MainCategories/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategories.FindAsync(id);
            if (mainCategory == null)
            {
                return NotFound();
            }
            return View(mainCategory);
        }

        // POST: MainCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("MainCategoryId,MainCategoryName,IsDeleted")] MainCategory mainCategory)
        {
            if (id != mainCategory.MainCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mainCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MainCategoryExists(mainCategory.MainCategoryId))
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
            return View(mainCategory);
        }

        // GET: MainCategories/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mainCategory = await _context.MainCategories
                .FirstOrDefaultAsync(m => m.MainCategoryId == id);
            if (mainCategory == null)
            {
                return NotFound();
            }

            return View(mainCategory);
        }

        // POST: MainCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var mainCategory = await _context.MainCategories.FindAsync(id);
            _context.MainCategories.Remove(mainCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MainCategoryExists(long id)
        {
            return _context.MainCategories.Any(e => e.MainCategoryId == id);
        }
    }
}
