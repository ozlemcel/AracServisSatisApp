using AracServisSatis.Entities.Concrete;
using AracServisSatis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AracServisSatis.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Admin" });

            var model = new EditProfileViewModel
            {
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Admin" });

            user.Name = model.FirstName;
            user.Surname = model.LastName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Şifre değiştirilecekse
            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                // Şifre değiştiyse yeniden giriş yapması gerekebilir
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.Success = "Bilgileriniz başarıyla güncellendi.";
            }
            else
            {
                ViewBag.Error = "Güncelleme sırasında bir hata oluştu.";
            }

            return View(model);
        }
    }
}
