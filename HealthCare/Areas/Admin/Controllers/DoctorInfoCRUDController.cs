using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Data;
using HealthCare.Entities;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DoctorInfoCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorInfoCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DoctorInfoCRUD
        public async Task<IActionResult> Index(string userId)
        {
            if (userId != null)
            {
                List<DoctorInfo> list = await (from doctorInfo in _context.DoctorInfo
                                               where doctorInfo.userId == userId
                                               select new DoctorInfo
                                               {
                                                   doctorInfoId = doctorInfo.doctorInfoId,
                                                   userId = doctorInfo.userId,
                                                   fullName = doctorInfo.fullName,
                                                   email = doctorInfo.email,
                                                   phoneNumber = doctorInfo.phoneNumber,
                                                   birthday = doctorInfo.birthday,
                                                   gender = doctorInfo.gender,
                                                   address = doctorInfo.address,
                                                   qualification = doctorInfo.qualification,
                                                   specialtyId = doctorInfo.specialtyId,
                                                   about = doctorInfo.about,
                                                   meta = doctorInfo.meta,
                                                   status = doctorInfo.status,
                                                   createAt = doctorInfo.createAt,
                                                   updateAt = doctorInfo.updateAt
                                               }).ToListAsync();
                ViewBag.userId = userId;

                return View(list);
            }

            return View(await _context.DoctorInfo.ToListAsync());
        }

        // GET: Admin/DoctorInfoCRUD/Create
        public IActionResult Create(string userId)
        {
            ViewBag.userId = userId;
            ViewBag.specialtyId = new SelectList(_context.Specialty, "specialtyId", "specialtyName");

            return View();
        }

        // POST: Admin/DoctorInfoCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("userId,fullName,qualification,specialtyId,phoneNumber,email,birthday,gender,address,about")] DoctorInfo doctorInfo)
        {
            if (ModelState.IsValid)
            {
                doctorInfo.meta = "";
                doctorInfo.status = true;
                doctorInfo.createAt = DateTime.Now;
                _context.DoctorInfo.Add(doctorInfo);
                if (await _context.SaveChangesAsync() > 0)
                {
                    ModelState.Clear();
                    TempData["msgCreate"] = "Successfully create a new product!";
                    return RedirectToAction(nameof(Index), new { userId = doctorInfo.userId });
                }
            }

            ViewBag.specialtyId = new SelectList(_context.Specialty, "specialtyId", "specialtyName", doctorInfo.specialtyId);

            return View(doctorInfo);
        }

        // GET: Admin/DoctorInfoCRUD/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DoctorInfo == null)
            {
                return NotFound();
            }

            var doctorInfo = await _context.DoctorInfo.FindAsync(id);
            if (doctorInfo == null)
            {
                return NotFound();
            }
            ViewBag.specialtyId = new SelectList(_context.Specialty, "specialtyId", "specialtyName", doctorInfo.specialtyId);

            return View(doctorInfo);
        }

        // POST: Admin/DoctorInfoCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("userId,doctorInfoId,fullName,qualification,specialtyId,phoneNumber,email,birthday,gender,address,about,status,meta,createAt")] DoctorInfo doctorInfo)
        {
            if (id != doctorInfo.doctorInfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorInfoExists(doctorInfo.doctorInfoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { userId = doctorInfo.userId });
            }
            ViewBag.specialtyId = new SelectList(_context.Specialty, "specialtyId", "specialtyName", doctorInfo.specialtyId);

            return View(doctorInfo);
        }

        // POST: Admin/DoctorInfoCRUD/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.DoctorInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.DoctorInfos'  is null.");
            }
            var doctorInfo = await _context.DoctorInfo.FindAsync(id);
            if (doctorInfo != null)
            {
                _context.DoctorInfo.Remove(doctorInfo);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        private bool DoctorInfoExists(int id)
        {
            return (_context.DoctorInfo?.Any(e => e.doctorInfoId == id)).GetValueOrDefault();
        }
    }
}
