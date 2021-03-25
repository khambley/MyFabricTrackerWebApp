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
    public class TransactionsController : Controller
    {
        private readonly FabricTrackerDbContext _context;

        public TransactionsController(FabricTrackerDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var fabricTrackerDbContext = _context.Transactions
                .Include(t => t.Fabric)
                .OrderByDescending(t => t.TransactionDate);
            //
            return View(await fabricTrackerDbContext.ToListAsync());

        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Fabric)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            long fabricId = 0;

            ViewData["FabricList"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", fabricId);

            ViewData["FabricId"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", fabricId);
            
            // 1. Create a new list to store all transactions with given FabricId
            List <Transaction> transactionListbyFabricId = new List<Transaction>();

            // 2. Select all transactions with given FabricId
            transactionListbyFabricId = (from t in _context.Transactions
                                         where t.FabricId == fabricId
                                         select t).ToList();

            // 3. Sum up InchesQty field in List
            ViewBag.TotalInches = transactionListbyFabricId.Sum(t => t.InchesQty);
            return View();
        }
        public JsonResult GetQuantityOnHand(int fabricId)
        {
            // Get data from database
            Fabric fabric = _context.Fabrics
                    .Where(f => f.FabricID == fabricId)
                    .FirstOrDefault();
            var totalQty = fabric.TotalInches;
            return Json(totalQty);
        }
        
        [HttpPost]
        public IActionResult ShowImage(long fabricId)
		{

            Fabric fabric = _context.Fabrics
                    .Where(f => f.FabricID == fabricId)
                    .FirstOrDefault();
            ViewBag.SelectedFabricImage = fabric.ImageFileName;
            ViewData["FabricList"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", fabricId);
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", fabricId);
            return View("Create");


        }
        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,FabricId,InchesQty,FatQtrQty,Reason,TransactionDate,IsDeleted")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var fabricToUpdate = await _context.Fabrics
                    .Where(f => f.FabricID == transaction.FabricId)
                    .FirstOrDefaultAsync();
                
                long? totalInches = 0;

                if (fabricToUpdate.TotalInches == null)
                {
                    totalInches = transaction.InchesQty;
                }
                else
                {
                    totalInches = fabricToUpdate.TotalInches + transaction.InchesQty;
                }
                fabricToUpdate.TotalInches = totalInches;
                fabricToUpdate.DateModified = transaction.TransactionDate;
                if (await TryUpdateModelAsync<Fabric>(fabricToUpdate,
                    "",
                    f => f.TotalInches, f => f.DateModified))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException /*ex*/)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", transaction.FabricId);

            // 1. Create a new list to store all transaction with given FabricId
            List<Transaction> transactionListbyFabricId = new List<Transaction>();

            // 2. Select all transactions with given FabricId
            transactionListbyFabricId = (from t in _context.Transactions
                                         where t.FabricId == transaction.FabricId
                                         select t).ToList();

            // 3. Sum up InchesQty field in List
            ViewBag.TotalInches = transactionListbyFabricId.Sum(t => t.InchesQty);


            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", transaction.FabricId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("TransactionId,FabricId,InchesQty,FatQtrQty,Reason,TransactionDate,IsDeleted")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["FabricId"] = new SelectList(_context.Fabrics, "FabricID", "FabricName", transaction.FabricId);
            return View(transaction);
        }
        public IEnumerable<Transaction> GetFabricAdjustments(int fabricId)
        {
            return _context.Transactions
                .Where(f => f.FabricId == fabricId)
                .ToList();
        }
        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Fabric)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(long id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}
