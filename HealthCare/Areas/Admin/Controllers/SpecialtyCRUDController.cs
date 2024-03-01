using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data;
using HealthCare.Entities;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialtyCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecialtyCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SpecialtyCRUD
        public async Task<IActionResult> Index()
        {
            return View(await _context.Specialty.ToListAsync());
        }

        // GET: Admin/SpecialtyCRUD/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SpecialtyCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("specialtyName,order,meta")] Specialty specialty)
        
        {
            if (ModelState.IsValid)
            {
                specialty.status = true;
                specialty.createAt = DateTime.Now;
                _context.Specialty.Add(specialty);
                if (await _context.SaveChangesAsync() > 0)
                {
                    ModelState.Clear();
                    TempData["msgCreate"] = "Successfully create a new product!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(specialty);
        }

        // GET: Admin/SpecialtyCRUD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialty == null)
            {
                return NotFound();
            }

            var specialty = await _context.Specialty.FindAsync(id);
            if (specialty == null)
            {
                return NotFound();
            }
            return View(specialty);
        }

        // POST: Admin/SpecialtyCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("specialtyId,specialtyName,order,meta,status,createAt")] Specialty specialty)
        {
            if (id != specialty.specialtyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialtyExists(specialty.specialtyId))
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
            return View(specialty);
        }

        // POST: Admin/SpecialtyCRUD/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Specialty == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Specialties'  is null.");
            }
            var specialty = await _context.Specialty.FindAsync(id);
            if (specialty != null)
            {
                _context.Specialty.Remove(specialty);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
                      
            return Json(new { success = false });
        }

        private bool SpecialtyExists(int id)
        {
          return (_context.Specialty?.Any(e => e.specialtyId == id)).GetValueOrDefault();
        }
    }
}
