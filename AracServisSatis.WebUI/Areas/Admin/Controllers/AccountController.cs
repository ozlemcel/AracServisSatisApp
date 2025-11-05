using AracServisSatis.Entities.Concrete;
using AracServisSatis.WebUI.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AracServisSatis.WebUI.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel p)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Name = p.Name,
                    Surname = p.Surname,
                    Email = p.Mail,
                    UserName = p.UserName,
                };

               

                var result = await _userManager.CreateAsync(user, p.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    TempData["SuccessMessage"] = "Üyelik işleminiz başarıyla gerçekleşmiştir.";
                    return RedirectToAction("Login");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(p);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel p)
        {
            
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, true, true);
                if (result.Succeeded)
               return RedirectToAction("Index", "Profile");
                                       
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz giriş bilgileri");
            }
           
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
