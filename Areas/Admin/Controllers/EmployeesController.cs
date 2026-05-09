using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PharmaSphere.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý nhân viên";
            return View();
        }
    }
}
