using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace PharmaSphere.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Thống kê báo cáo";
            return View();
        }
    }
}
