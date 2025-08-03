using Care_Pulse.Data;
using Care_Pulse.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Care_Pulse.Controllers
{
    public class AccountController : Controller
    {
        private readonly CarePulseContext _context;

        public AccountController(CarePulseContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password && u.IsActive);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetInt32("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.Name);

                return user.Role == 1
                    ? RedirectToAction("Dashboard", "Admin")
                    : RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "بيانات الدخول غير صحيحة";
            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مسجل مسبقاً");
                return View(user);
            }

            user.Role = 2;
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetInt32("UserRole", user.Role);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(
            [Bind("Id,Name,Email,Phone,Image,Bio")] User updatedUser,
            IFormFile? imageFile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId != updatedUser.Id)
                return RedirectToAction("Login");

            var existingUser = _context.Users.Find(userId);
            if (existingUser == null)
                return RedirectToAction("Login");

            // Email uniqueness check
            if (existingUser.Email != updatedUser.Email &&
                _context.Users.Any(u => u.Email == updatedUser.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني مسجل مسبقاً");
                return View("Profile", updatedUser);
            }

            // Update editable fields
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
             
            existingUser.Bio = updatedUser.Bio;

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Delete old image if it exists
                if (!string.IsNullOrEmpty(existingUser.Image))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingUser.Image.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingUser.Image = "/uploads/" + uniqueFileName;
            }

            _context.SaveChanges();

            // Update session data
            HttpContext.Session.SetString("UserName", existingUser.Name);

            ViewBag.Success = "تم تحديث الملف الشخصي بنجاح";
            return RedirectToAction("Profile");
        }

        // Add to your existing ChangePassword action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users.Find(userId);
            if (user == null)
                return RedirectToAction("Login");

            if (user.PasswordHash != currentPassword)
            {
                ModelState.AddModelError("currentPassword", "كلمة المرور الحالية غير صحيحة");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "كلمة المرور الجديدة وتأكيدها غير متطابقين");
                return View();
            }

            user.PasswordHash = newPassword;
            _context.SaveChanges();

            ViewBag.Success = "تم تغيير كلمة المرور بنجاح";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}