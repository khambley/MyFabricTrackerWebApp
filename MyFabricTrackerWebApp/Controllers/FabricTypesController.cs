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
    public class FabricTypesController : Controller
    {
        private readonly FabricTrackerDbContext _context;

        public FabricTypesController(FabricTrackerDbContext context)
        {
            _context = context;
        }

        // GET: FabricTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.FabricType.ToListAsync());
        }

        // GET: FabricTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricType = await _context.FabricType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabricType == null)
            {
                return NotFound();
            }

            return View(fabricType);
        }

        // GET: FabricTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FabricTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] FabricType fabricType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fabricType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fabricType);
        }

        // GET: FabricTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricType = await _context.FabricType.FindAsync(id);
            if (fabricType == null)
            {
                return NotFound();
            }
            return View(fabricType);
        }

        // POST: FabricTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] FabricType fabricType)
        {
            if (id != fabricType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabricType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricTypeExists(fabricType.Id))
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
            return View(fabricType);
        }

        // GET: FabricTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fabricType = await _context.FabricType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fabricType == null)
            {
                return NotFound();
            }

            return View(fabricType);
        }

        // POST: FabricTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabricType = await _context.FabricType.FindAsync(id);
            _context.FabricType.Remove(fabricType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FabricTypeExists(int id)
        {
            return _context.FabricType.Any(e => e.Id == id);
        }
    }
}
