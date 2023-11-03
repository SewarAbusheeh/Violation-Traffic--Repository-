using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Violation_p2.Models;

namespace Violation_p2.Controllers
{
    public class ViolationtypesController : Controller
    {
        private readonly ModelContext _context;

        public ViolationtypesController(ModelContext context)
        {
            _context = context;
        }

        // GET: Violationtypes
        public async Task<IActionResult> Index()
        {
              return _context.Violationtypes != null ? 
                          View(await _context.Violationtypes.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Violationtypes'  is null.");
        }

        // GET: Violationtypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Violationtypes == null)
            {
                return NotFound();
            }

            var violationtype = await _context.Violationtypes
                .FirstOrDefaultAsync(m => m.ViolationtypeId == id);
            if (violationtype == null)
            {
                return NotFound();
            }

            return View(violationtype);
        }

        // GET: Violationtypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Violationtypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ViolationtypeId,Description,Basefineamount")] Violationtype violationtype)
        {
            if (ModelState.IsValid)
            {
                _context.Add(violationtype);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(violationtype);
        }

        // GET: Violationtypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Violationtypes == null)
            {
                return NotFound();
            }

            var violationtype = await _context.Violationtypes.FindAsync(id);
            if (violationtype == null)
            {
                return NotFound();
            }
            return View(violationtype);
        }

        // POST: Violationtypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ViolationtypeId,Description,Basefineamount")] Violationtype violationtype)
        {
            if (id != violationtype.ViolationtypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(violationtype);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViolationtypeExists(violationtype.ViolationtypeId))
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
            return View(violationtype);
        }

        // GET: Violationtypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Violationtypes == null)
            {
                return NotFound();
            }

            var violationtype = await _context.Violationtypes
                .FirstOrDefaultAsync(m => m.ViolationtypeId == id);
            if (violationtype == null)
            {
                return NotFound();
            }

            return View(violationtype);
        }

        // POST: Violationtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Violationtypes == null)
            {
                return Problem("Entity set 'ModelContext.Violationtypes'  is null.");
            }
            var violationtype = await _context.Violationtypes.FindAsync(id);
            if (violationtype != null)
            {
                _context.Violationtypes.Remove(violationtype);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViolationtypeExists(int id)
        {
          return (_context.Violationtypes?.Any(e => e.ViolationtypeId == id)).GetValueOrDefault();
        }
    }
}
