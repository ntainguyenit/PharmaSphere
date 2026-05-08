using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PharmaSphere.Data;
using PharmaSphere.Models;

namespace PharmaSphere.Areas.Customer.Controllers
{
    /// <summary>
    /// Controller handling customer account operations like login, logout, and registration.
    /// </summary>
    [Area("Customer")]
    public class AccountController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AccountController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the login page.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// Processes the login request.
        /// </summary>
        /// <param name="model">The login credentials.</param>
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>
        /// <returns>A redirect to the return URL or the login view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // In a production app, use hashed passwords!
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username && u.PasswordHash == model.Password);

                if (user != null)
                {
                    if (user.Role == UserRole.Admin || user.Role == UserRole.Staff)
                    {
                        ModelState.AddModelError(string.Empty, "Tài khoản quản trị không thể đăng nhập tại đây.");
                        return View(model);
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("FullName", user.FullName ?? ""),
                        new Claim(ClaimTypes.Role, user.Role.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, "CustomerAuth");
                    var principal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : null
                    };

                    await HttpContext.SignInAsync("CustomerAuth", principal, authProperties);

                    return LocalRedirect(returnUrl ?? "/");
                }
                
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
            }

            return View(model);
        }

        /// <summary>
        /// Logs out the current user.
        /// </summary>
        /// <returns>A redirect to the home page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CustomerAuth");
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        /// <summary>
        /// Displays the registration page.
        /// </summary>
        /// <returns>The registration view.</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Processes the registration request.
        /// </summary>
        /// <param name="model">The registration data.</param>
        /// <returns>A redirect to the login page upon success.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // In a real app, save to database here
                return RedirectToAction("Login");
            }
            return View(model);
        }

        /// <summary>
        /// Displays the forgot password page.
        /// </summary>
        /// <returns>The forgot password view.</returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
