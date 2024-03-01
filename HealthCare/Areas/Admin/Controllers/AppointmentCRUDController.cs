using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthCare.Areas.Admin.Models;
using HealthCare.Data;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HealthCare.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AppointmentCRUDController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: AppointmentCRUDController
        public ActionResult Index()
        {
            var appointments = _context.Appoiment.ToList();

            return View(appointments);
        }

        // GET: AppointmentCRUDController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppointmentCRUDController/Create
        public ActionResult Create(string userId)
        {
            ViewBag.userId = userId;
            ViewBag.DoctorInfoId = new SelectList(_context.DoctorInfo, "doctorInfoId", "fullname");
            return View();
        }

        // POST: AppointmentCRUDController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("appDate,reason,description,doctorId,patientId,meta")] Appoiment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.status = true;
                appointment.createAt = DateTime.Now;

                _context.Appoiment.Add(appointment);

                if (await _context.SaveChangesAsync() > 0)
                {
                    ModelState.Clear();
                    TempData["msgCreate"] = "Successfully create a new appointment!";
                    return RedirectToAction(nameof(Index), new { userId = appointment.doctorId });
                }
            }

            return View(appointment);
        }


        // GET: AppointmentCRUDController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AppointmentCRUDController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AppointmentCRUDController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AppointmentCRUDController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
