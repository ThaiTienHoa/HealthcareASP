using HealthCare.Data;
using HealthCare.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UserAuthController(ApplicationDbContext context,
                                  UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Login()
        {          
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            loginModel.loginInValid = "true";

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.phoneNumber,
                                                                    loginModel.password, false, false);
                if (result.Succeeded)
                {
                    loginModel.loginInValid = "";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Xác thực không thành công");
                }
            }

            return PartialView(loginModel);
        }

        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationModel registrationModel)
        {
            registrationModel.registrationInValid = "true";

            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = registrationModel.phoneNumber,
                    PhoneNumber = registrationModel.phoneNumber,
                    firstName = registrationModel.firstName,
                    lastName = registrationModel.lastName,
                    avatar = "~/img/avatar/user.png",
                    meta = "",
                    status = true,
                    createAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registrationModel.password);

                if (result.Succeeded)
                {
                    registrationModel.registrationInValid = "";

                    _userManager.AddToRoleAsync(user, "Patient").Wait();
                    await _signInManager.SignInAsync(user, false);

                    /*if (registrationModel.CategoryId != 0)
                    {
                        await AddCategoryToUser(user.Id, registrationModel.CategoryId);*/

                    /* return RedirectToAction("Index", "Home");*/
                    /* }*/

                    return PartialView(registrationModel);
                }

                AddErrorsToModelState(result);

            }
            return PartialView(registrationModel);
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> UpdateProfile()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (userId == null)
            {
                return RedirectToAction("Login", "UserAuth");
            }

            var user = await (from u in _context.Users
                              where u.Id == userId
                              select new UserModel
                              {
                                  Id = u.Id,
                                  PhoneNumber = u.PhoneNumber,
                                  firstName = u.firstName,
                                  lastName = u.lastName,
                                  avatar = u.avatar
                              }).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                ApplicationUser user = _userManager.FindByIdAsync(userId).Result;

                if (user != null)
                {
                    user.PhoneNumber = userModel.PhoneNumber;
                    user.firstName = userModel.firstName;
                    user.lastName = userModel.lastName;
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
                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                            userModel.avatar = user.avatar;
                            return View(userModel);
                        }
                    }
                    else
                    {
                        IdentityResult x = await _userManager.UpdateAsync(user);

                        userModel.avatar = user.avatar;
                        if (x.Succeeded)
                        {
                            return View(userModel);
                        }
                    }
                }
            }

            return View(userModel);
        }

        public async Task<bool> UserNameExists(string userName)
        {
            bool userNameExists = await _context.Users.AnyAsync(u => u.UserName.ToUpper() == userName.ToUpper());

            if (userNameExists)
                return true;

            return false;
        }

        private void AddErrorsToModelState(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}