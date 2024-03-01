using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data;
using HealthCare.Entities;
using Microsoft.CodeAnalysis;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecializationCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SpecializationCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SpecializationCRUD?specialtyId=5
        public async Task<IActionResult> Index(int specialtyId)
        {
            if (specialtyId != 0)
            {
                List<Specialization> list = await (from specialization in _context.Specialization
                                                   where specialization.specialtyId == specialtyId
                                                   select new Specialization
                                                   {
                                                       specializationId = specialization.specialtyId,
                                                       specializationName = specialization.specializationName,
                                                       specialtyId = specialtyId,
                                                       order = specialization.order,
                                                       meta = specialization.meta,
                                                       status = specialization.status,
                                                       createAt = specialization.createAt,
                                                       updateAt = specialization.updateAt
                                                   }).ToListAsync();
                ViewBag.specialtyId = specialtyId;

                return View(list);
            }

            return View(await _context.Specialization.ToListAsync());
        }

        // GET: Admin/SpecializationCRUD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Specialization == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specialization
                .FirstOrDefaultAsync(m => m.specializationId == id);
            if (specialization == null)
            {
                return NotFound();
            }

            return View(specialization);
        }

        // GET: Admin/SpecializationCRUD/Create
        public IActionResult Create(int specialtyId)
        {
            ViewBag.specialtyId = specialtyId;

            return View();
        }

        // POST: Admin/SpecializationCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("specializationName,specialtyId,order,meta")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                specialization.status = true;
                specialization.createAt = DateTime.Now;
                _context.Add(specialization);
                if (await _context.SaveChangesAsync() > 0)
                {
                    ModelState.Clear();
                    TempData["msgCreate"] = "Successfully create a new product!";
                    return RedirectToAction(nameof(Index), new { specialtyId = specialization.specialtyId });
                }
            }
            return View(specialization);
        }

        // GET: Admin/SpecializationCRUD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Specialization == null)
            {
                return NotFound();
            }

            var specialization = await _context.Specialization.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }
            return View(specialization);
        }

        // POST: Admin/SpecializationCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("specializationId,specializationName,specialtyId,order,meta,status,createAt")] Specialization specialization)
        {
            if (id != specialization.specializationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecializationExists(specialization.specializationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { specialtyId = specialization.specialtyId });
            }
            return View(specialization);
        }

        // POST: Admin/SpecializationCRUD/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Specialization == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Specialization'  is null.");
            }
            var specialization = await _context.Specialization.FindAsync(id);
            if (specialization != null)
            {
                _context.Specialization.Remove(specialization);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        private bool SpecializationExists(int id)
        {
            return (_context.Specialization?.Any(e => e.specializationId == id)).GetValueOrDefault();
        }
    }
}
