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
    public class ViolationsController : Controller
    {
        private readonly ModelContext _context;

        public ViolationsController(ModelContext context)
        {
            _context = context;
        }

        // GET: Violations
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Violations
    .Include(v => v.Vehicle)
    .ThenInclude(vehicle => vehicle.User) // Including the User related to the Vehicle
    .Include(v => v.Violationtype)
    .ToList();

            return View(modelContext);
        }

        // GET: Violations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Violations == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations
                .Include(v => v.Vehicle)
                .Include(v => v.Violationtype)
                .FirstOrDefaultAsync(m => m.ViolationId == id);
            if (violation == null)
            {
                return NotFound();
            }

            return View(violation);
        }

        // GET: Violations/Create
        public IActionResult Create()
        {
            ViewData["Vehicles"] = new SelectList(_context.Vehicles, "VehicleId", "Licenseplate");
            // This will display the Description but use ViolationtypeId as the value
            ViewData["Violationtypes"] = new SelectList(_context.Violationtypes, "ViolationtypeId", "Description");

            return View();
        }

        // POST: Violations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ViolationId,VehicleId,ViolationtypeId,Violationdate,Fineamount,Ispaid")] Violation violation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(violation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Populate the dropdown with Licenseplate for display and VehicleId for value.
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleId", violation.VehicleId);

            ViewData["ViolationtypeId"] = new SelectList(_context.Violationtypes, "ViolationtypeId", "ViolationtypeId", violation.ViolationtypeId);
            return View(violation);
        }

        // GET: Violations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Violations == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations.FindAsync(id);
            if (violation == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleId", violation.VehicleId);
            ViewData["ViolationtypeId"] = new SelectList(_context.Violationtypes, "ViolationtypeId", "ViolationtypeId", violation.ViolationtypeId);
            return View(violation);
        }

        // POST: Violations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ViolationId,VehicleId,ViolationtypeId,Violationdate,Fineamount,Ispaid")] Violation violation)
        {
            if (id != violation.ViolationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(violation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViolationExists(violation.ViolationId))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "VehicleId", "VehicleId", violation.VehicleId);
            ViewData["ViolationtypeId"] = new SelectList(_context.Violationtypes, "ViolationtypeId", "ViolationtypeId", violation.ViolationtypeId);
            return View(violation);
        }

        // GET: Violations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Violations == null)
            {
                return NotFound();
            }

            var violation = await _context.Violations
                .Include(v => v.Vehicle)
                .Include(v => v.Violationtype)
                .FirstOrDefaultAsync(m => m.ViolationId == id);
            if (violation == null)
            {
                return NotFound();
            }

            return View(violation);
        }

        // POST: Violations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Violations == null)
            {
                return Problem("Entity set 'ModelContext.Violations'  is null.");
            }
            var violation = await _context.Violations.FindAsync(id);
            if (violation != null)
            {
                _context.Violations.Remove(violation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViolationExists(int id)
        {
            return (_context.Violations?.Any(e => e.ViolationId == id)).GetValueOrDefault();
        }
    }
}
