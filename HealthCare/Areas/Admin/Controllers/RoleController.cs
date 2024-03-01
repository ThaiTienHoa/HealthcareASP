using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCare.Areas.Admin.Models;
using HealthCare.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: Admin/Role
        public async Task<IActionResult> Index()
        {
            return View(await (from role in _context.Roles
                               select new RoleModel
                               {
                                   Id = role.Id,
                                   roleName = role.Name
                               }).ToListAsync());
        }

        // GET: Admin/Role/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.RoleModel == null)
            {
                return NotFound();
            }

            var roleModel = await _context.RoleModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleModel == null)
            {
                return NotFound();
            }

            return View(roleModel);
        }

        // GET: Admin/Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleModel.roleName);

            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleModel.roleName));

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsToModelState(result);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role này đã tồn tại");
            }

            return View(roleModel);
        }

        // GET: Admin/Role/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var roleModel = await (from role in _context.Roles
                                   where role.Id == id
                                   select new RoleModel
                                   {
                                       Id = role.Id,
                                       roleName = role.Name
                                   }).FirstOrDefaultAsync();
            if (roleModel == null)
            {
                return NotFound();
            }
            return View(roleModel);
        }

        // POST: Admin/Role/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleModel roleModel)
        {
            if (id != roleModel.Id)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleModel.Id);

            if (role != null)
            {
                role.Name = roleModel.roleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                AddErrorsToModelState(result);
            }

            return View(roleModel);
        }

        // POST: Admin/Role/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.RoleModel'  is null.");
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }

                AddErrorsToModelState(result);
            }

            return Json(new { success = false });
        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
