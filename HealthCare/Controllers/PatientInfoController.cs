using HealthCare.Data;
using HealthCare.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Controllers
{
    [Authorize(Roles = "Admin, Patient")]
    public class PatientInfoController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public PatientInfoController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

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
                return View(list);
            }

            return RedirectToAction("Login", "UserAuth");
        }

        // GET: Admin/PatientInfoCRUD/Create
        public IActionResult Create()
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            if (userId != null)
            {
                ViewBag.userId = userId;
            }
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
                    return RedirectToAction(nameof(Index));
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PatientInfo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PatientInfos'  is null.");
            }
            var patientInfo = await _context.PatientInfo.FindAsync(id);
            if (patientInfo != null)
            {
                _context.PatientInfo.Remove(patientInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatientInfoExists(int id)
        {
            return (_context.PatientInfo?.Any(e => e.patientInfoId == id)).GetValueOrDefault();
        }
    }
}
