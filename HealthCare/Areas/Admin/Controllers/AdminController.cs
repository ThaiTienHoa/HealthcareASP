using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthCare.Areas.Admin.Models;
using HealthCare.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace HealthCare.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Admin/UserCRUD
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();

            var userModelList = new List<UserModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault();

                var userModel = new UserModel
                {
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    role = role,
                    firstName = user.firstName,
                    lastName = user.lastName,
                    avatar = user.avatar,
                    status = user.status,
                    updateAt = user.updateAt
                };

                userModelList.Add(userModel);
            }

            var sortedUserModelList = userModelList.OrderByDescending(u => u.updateAt).ToList();

            return View(sortedUserModelList);
        }

        // GET: Admin/UserCRUD/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // GET: Admin/UserCRUD/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.role = new SelectList(await (from role in _context.Roles
                                                 select new RoleModel
                                                 {
                                                     Id = role.Id,
                                                     roleName = role.Name,
                                                 }).ToListAsync(), "Id", "roleName");
            return View();
        }

        // POST: Admin/UserCRUD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("phoneNumber,firstName,lastName,role,password,confirmPassword")] CreateAccountModel createAccountModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = createAccountModel.phoneNumber,
                    PhoneNumber = createAccountModel.phoneNumber,
                    firstName = createAccountModel.firstName,
                    lastName = createAccountModel.lastName,
                    avatar = "~/img/avatar/user.png",
                    meta = "",
                    status = true,
                    createAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, createAccountModel.password);

                if (result.Succeeded)
                {
                    var roleName = (await _roleManager.FindByIdAsync(createAccountModel.role))?.Name;

                    if (roleName != null)
                    {
                        await _userManager.AddToRoleAsync(user, roleName);
                    }

                    /*if (registrationModel.CategoryId != 0)
                    {
                        await AddCategoryToUser(user.Id, registrationModel.CategoryId);*/

                    /* return RedirectToAction("Index", "Home");*/
                    /* }*/

                    return RedirectToAction(nameof(Index));
                }

                AddErrorsToModelState(result);

            }
            ViewBag.role = new SelectList(await (from role in _context.Roles
                                                 select new RoleModel
                                                 {
                                                     Id = role.Id,
                                                     roleName = role.Name,
                                                 }).ToListAsync(), "Id", "roleName");
            return View(createAccountModel);
        }

        // GET: Admin/UserCRUD/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            var roleId = (await _roleManager.FindByNameAsync(role))?.Id;


            var userModel = new UserModel
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                role = roleId,
                firstName = user.firstName,
                lastName = user.lastName,
                avatar = user.avatar,
                status = user.status,
                createAt = user.createAt,
                updateAt = user.updateAt
            };

            ViewBag.role = new SelectList(await (from r in _context.Roles
                                                 select new RoleModel
                                                 {
                                                     Id = r.Id,
                                                     roleName = r.Name,
                                                 }).ToListAsync(), "Id", "roleName", roleId);
            return View(userModel);
        }

        // POST: Admin/UserCRUD/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,PhoneNumber,firstName,lastName,role,avatar,status,createAt")] UserModel userModel)
        {
            if (id != userModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.FindByIdAsync(userModel.Id).Result;

                if (user != null)
                {
                    user.PhoneNumber = userModel.PhoneNumber;
                    user.firstName = userModel.firstName;
                    user.lastName = userModel.lastName;
                    user.status = userModel.status;
                    user.updateAt = DateTime.Now;

                    if (Request.Form.Files.Count > 0)
                    {
                        IFormFile file = Request.Form.Files.FirstOrDefault();

                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "img", "avatar", fileName);


                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        string oldImgPath = Path.Combine(_hostingEnvironment.WebRootPath, user.avatar.TrimStart('~').TrimStart('/'));
                        user.avatar = "~/img/avatar/" + fileName;

                        IdentityResult x = await _userManager.UpdateAsync(user);

                        if (x.Succeeded)
                        {
                            await UpdateRoleAsync(userModel.role, user);

                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                            userModel.avatar = user.avatar;

                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        IdentityResult x = await _userManager.UpdateAsync(user);

                        userModel.avatar = user.avatar;
                        if (x.Succeeded)
                        {
                            await UpdateRoleAsync(userModel.role, user);
                            
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(userModel);
        }

        // GET: Admin/UserCRUD/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.UserModel == null)
            {
                return NotFound();
            }

            var userModel = await _context.UserModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userModel == null)
            {
                return NotFound();
            }

            return View(userModel);
        }

        // POST: Admin/UserCRUD/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.UserModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserModel'  is null.");
            }
            var userModel = await _context.UserModel.FindAsync(id);
            if (userModel != null)
            {
                _context.UserModel.Remove(userModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserModelExists(string id)
        {
            return (_context.UserModel?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task UpdateRoleAsync(string? id, ApplicationUser user)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (currentRoles.Any())
                {
                    // Xóa người dùng khỏi các vai trò hiện tại
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                }

                // Thêm người dùng vào vai trò mới
                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}
