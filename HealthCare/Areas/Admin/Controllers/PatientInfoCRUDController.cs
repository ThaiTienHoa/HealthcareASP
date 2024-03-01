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
    public class PatientInfoCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PatientInfoCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PatientInfoCRUD
        public async Task<IActionResult> Index(string userId)
        {
            if (userId != null)
            {
                List<PatientInfo> list = await (from patientInfo in _context.PatientInfo
                                                where patientInfo.userId == userId
                                                select new PatientInfo
                                                {
                                                    patientInfoId = patientInfo.patientInfoId,
                                                    fullName = patientInfo.fullName,
                                                    phoneNumber = patientInfo.phoneNumber,
                                                    birthday = patientInfo.birthday,
                                                    gender = patientInfo.gender,
                                                    insurance = patientInfo.insurance,
                                                    address = patientInfo.address,
                                                    nationalId = patientInfo.nationalId,
                                                    job = patientInfo.job,
                                                    userId = patientInfo.userId,
                                                    meta = patientInfo.meta,
                                                    status = patientInfo.status,
                                                    createAt = patientInfo.createAt,
                                                    updateAt = patientInfo.updateAt
                                                }).ToListAsync();
                ViewBag.userId = userId;

                return View(list);
            }

            return View(await _context.PatientInfo.ToListAsync());
        }


        // GET: Admin/PatientInfoCRUD/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PatientInfo == null)
            {
                return NotFound();
            }

            var patientInfo = await _context.PatientInfo
                .FirstOrDefaultAsync(m => m.patientInfoId == id);
            if (patientInfo == null)
            {
                return NotFound();
            }

            return View(patientInfo);
        }

        // GET: Admin/PatientInfoCRUD/Create
        public IActionResult Create(string userId)
        {
            ViewBag.userId = userId;

            return View();
        }

        // POST: Admin/PatientInfoCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fullName,phoneNumber,birthday,gender,insurance,address,nationalId,job,userId")] PatientInfo patientInfo)
        {
            if (ModelState.IsValid)
            {
                patientInfo.status = true;
                patientInfo.createAt = DateTime.Now;
                _context.Add(patientInfo);
                if (await _context.SaveChangesAsync() > 0)
                {
                    ModelState.Clear();
                    TempData["msgCreate"] = "Successfully create a new product!";
                    return RedirectToAction(nameof(Index), new { userId = patientInfo.userId });
                }
            }
            return View(patientInfo);
        }

        // GET: Admin/PatientInfoCRUD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PatientInfo == null)
            {
                return NotFound();
            }

            var patientInfo = await _context.PatientInfo.FindAsync(id);
            if (patientInfo == null)
            {
                return NotFound();
            }
            return View(patientInfo);
        }

        // POST: Admin/PatientInfoCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("patientInfoId,fullName,phoneNumber,birthday,gender,insurance,address,nationalId,job,userId,status,createAt")] PatientInfo patientInfo)
        {
            if (id != patientInfo.patientInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patientInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientInfoExists(patientInfo.patientInfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { userId = patientInfo.userId });
            }
            return View(patientInfo);
        }

        // POST: Admin/PatientInfoCRUD/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.PatientInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientInfos'  is null.");
            }
            var patientInfo = await _context.PatientInfo.FindAsync(id);
            if (patientInfo != null)
            {
                _context.PatientInfo.Remove(patientInfo);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        private bool PatientInfoExists(int id)
        {
            return (_context.PatientInfo?.Any(e => e.patientInfoId == id)).GetValueOrDefault();
        }
    }
}
