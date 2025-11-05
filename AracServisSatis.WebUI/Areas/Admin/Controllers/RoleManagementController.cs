using AracServisSatis.Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AracServisSatis.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleManagementController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleManagementController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // 🔹 1. Tüm kullanıcıları listele
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // 🔹 2. Roller listesini görüntüle
        public IActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        // 🔹 3. Yeni rol oluştur
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var result = await _roleManager.CreateAsync(new AppRole(roleName));
                if (result.Succeeded)
                {
                    TempData["Success"] = "Rol başarıyla eklendi.";
                    return RedirectToAction("Roles");
                }
            }
            TempData["Error"] = "Rol eklenemedi.";
            return RedirectToAction("Roles");
        }

        // 🔹 4. Kullanıcıya rol ata
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.User = user;
            ViewBag.UserRoles = userRoles;

            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                TempData["Error"] = "Böyle bir rol yok.";
                return RedirectToAction("AssignRole", new { userId });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.UserName} kullanıcısına '{roleName}' rolü eklendi.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Rol atama başarısız.";
            return RedirectToAction("AssignRole", new { userId });
        }

        // 🔹 5. Kullanıcıdan rol sil
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                TempData["Success"] = "Rol başarıyla kaldırıldı.";
                return RedirectToAction("AssignRole", new { userId });
            }

            TempData["Error"] = "Rol kaldırılamadı.";
            return RedirectToAction("AssignRole", new { userId });
        }

        // 🔹 6. Kullanıcı silme işlemi (rolleriyle birlikte)
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // Kullanıcının mevcut rollerini al
            var roles = await _userManager.GetRolesAsync(user);

            // Kullanıcıyı tüm rollerden çıkar
            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            // Kullanıcıyı tamamen sil
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = $"{user.UserName} adlı kullanıcı ve tüm rolleri başarıyla silindi.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Kullanıcı silinirken bir hata oluştu.";
            return RedirectToAction("Index");
        }

    }
}
