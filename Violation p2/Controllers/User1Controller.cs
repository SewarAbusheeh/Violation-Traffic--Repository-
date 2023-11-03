using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Violation_p2.Models;

namespace Violation_p2.Controllers
{
    public class User1Controller : Controller
    {
        private readonly ModelContext _context;


        private readonly IWebHostEnvironment _webHostEnviroment;

        public User1Controller(ModelContext context, IWebHostEnvironment webHostEnviroment)
        {
            _context = context;
            _webHostEnviroment = webHostEnviroment;
        }
     

        // GET: User1
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.User1s.Include(u => u.Role);
            return View(await modelContext.ToListAsync());
        }

        // GET: User1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User1s == null)
            {
                return NotFound();
            }

            var user1 = await _context.User1s
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.User1Id == id);
            if (user1 == null)
            {
                return NotFound();
            }

            return View(user1);
        }

        // GET: User1/Create
        public IActionResult Create()
        {
            ViewData["Rolename"] = new SelectList(_context.Roles, "Rolename", "Rolename");
            return View();
        }

        // POST: User1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("User1Id,Username,Password,Email,Imagepath,RoleId")] User1 user1)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user1);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user1.RoleId);
            return View(user1);
        }

        // GET: User1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User1s == null)
            {
                return NotFound();
            }

            var user1 = await _context.User1s.FindAsync(id);
            if (user1 == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user1.RoleId);
            return View(user1);
        }

        // POST: User1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("User1Id,Username,Password,Email,Imagepath,RoleId")] User1 user1)
        {
            if (id != user1.User1Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                if (user1.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnviroment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + user1.ImageFile.FileName;



                    string path = Path.Combine(wwwRootPath + "/ImagesTodisplay/" + fileName);



                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await user1.ImageFile.CopyToAsync(fileStream);
                    }



                    user1.Imagepath = fileName;
                }
                try
                {
                    _context.Update(user1);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!User1Exists(user1.User1Id))
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
            ViewData["RoleId"] = new SelectList(_context.Roles, "RoleId", "RoleId", user1.RoleId);
            return View(user1);
        }

        // GET: User1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User1s == null)
            {
                return NotFound();
            }

            var user1 = await _context.User1s
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.User1Id == id);
            if (user1 == null)
            {
                return NotFound();
            }

            return View(user1);
        }

        // POST: User1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User1s == null)
            {
                return Problem("Entity set 'ModelContext.User1s'  is null.");
            }
            var user1 = await _context.User1s.FindAsync(id);
            if (user1 != null)
            {
                _context.User1s.Remove(user1);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool User1Exists(int id)
        {
          return (_context.User1s?.Any(e => e.User1Id == id)).GetValueOrDefault();
        }
    }
}
