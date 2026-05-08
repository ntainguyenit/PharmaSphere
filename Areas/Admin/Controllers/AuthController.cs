using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PharmaSphere.Data;
using PharmaSphere.Models;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for handling authentication in the Admin area.
    /// </summary>
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AuthController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the admin login page.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>
        /// <returns>The admin login view.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = "/Admin")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Processes the admin login request.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>
        /// <returns>A redirect to the return URL or the login view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/Admin")
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // In a production app, use hashed passwords!
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);

                if (user != null)
                {
                    if (user.Role == UserRole.Customer)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản khách hàng không được phép truy cập khu vực này.");
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("FullName", user.FullName ?? ""),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, "AdminAuth");
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                    };

                    await HttpContext.SignInAsync("AdminAuth", principal, authProperties);

                    return LocalRedirect(returnUrl ?? "/Admin");
                }
                
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            return View(model);
        }

        /// <summary>
        /// Logs out the admin user.
        /// </summary>
        /// <returns>A redirect to the admin login page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }
    }
}
