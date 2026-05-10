using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmaSphere.Data;
using PharmaSphere.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using BCrypt.Net;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller responsible for managing the administrator's profile, including information updates and password changes.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class ProfileController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ProfileController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the profile page for the currently logged-in administrator.
        /// </summary>
        /// <returns>A view displaying the administrator's current information.</returns>
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdStr);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            return View(user);
        }

        /// <summary>
        /// Processes a request to change the administrator's password with BCrypt hashing.
        /// </summary>
        /// <param name="currentPassword">The administrator's existing password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <param name="confirmPassword">Confirmation of the new password.</param>
        /// <returns>A JSON response indicating success or failure of the password update.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Phiên đăng nhập hết hạn." });

            int userId = int.Parse(userIdStr);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return Json(new { success = false, message = "Người dùng không tồn tại." });

            // Verify current password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                return Json(new { success = false, message = "Mật khẩu hiện tại không chính xác." });
            }

            if (newPassword != confirmPassword)
            {
                return Json(new { success = false, message = "Mật khẩu mới không khớp." });
            }

            // Hash the new password using BCrypt
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Đổi mật khẩu thành công!" });
        }

        /// <summary>
        /// Updates the administrator's personal information.
        /// </summary>
        /// <param name="fullName">The new full name.</param>
        /// <param name="email">The new email address.</param>
        /// <param name="phone">The new phone number.</param>
        /// <returns>A redirect to the profile index page with a success message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(string fullName, string email, string phone)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Auth");

            int userId = int.Parse(userIdStr);
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return NotFound();

            user.FullName = fullName;
            user.Email = email;
            user.Phone = phone;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Cập nhật thông tin thành công!";

            return RedirectToAction("Index");
        }
    }
}
