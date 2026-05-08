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
                        TempData["Error"] = "Đăng nhập thất bại: Tài khoản quản trị không thể đăng nhập tại đây.";
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

                    TempData["Success"] = $"Chào mừng {user.FullName ?? user.Username} quay trở lại!";
                    return LocalRedirect(returnUrl ?? "/");
                }
                
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                TempData["Error"] = "Đăng nhập thất bại: Tên đăng nhập hoặc mật khẩu không chính xác.";
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
            TempData["Success"] = "Bạn đã đăng xuất thành công.";
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã có người sử dụng.");
                    TempData["Error"] = "Đăng ký thất bại: Tên đăng nhập đã tồn tại.";
                    return View(model);
                }

                // Check if email already exists
                if (_context.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email này đã được đăng ký.");
                    TempData["Error"] = "Đăng ký thất bại: Email đã được sử dụng.";
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = model.Password, // WARNING: Plain text for demo purposes as per current codebase
                    FullName = model.FullName,
                    Email = model.Email,
                    Role = UserRole.Customer
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Đăng ký tài khoản thành công! Bạn có thể đăng nhập ngay bây giờ.";
                return RedirectToAction("Login");
            }
            
            TempData["Error"] = "Đăng ký thất bại: Vui lòng kiểm tra lại thông tin nhập liệu.";
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

        /// <summary>
        /// Processes the forgot password request.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>The forgot password view with success or error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Vui lòng nhập địa chỉ email.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                TempData["Success"] = "Yêu cầu đã được gửi! Vui lòng kiểm tra email để nhận hướng dẫn khôi phục mật khẩu.";
                return View();
            }

            TempData["Error"] = "Thất bại: Không tìm thấy tài khoản nào với email này trong hệ thống.";
            return View();
        }
    }
}
