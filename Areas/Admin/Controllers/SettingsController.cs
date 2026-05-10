using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmaSphere.Data;
using PharmaSphere.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller responsible for managing system-wide configuration settings. Restricted to Administrators.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth", Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SettingsController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves and displays all system configuration settings.
        /// </summary>
        /// <returns>A view displaying a list of current system settings.</returns>
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Cài đặt hệ thống";
            var settings = await _context.SystemSettings.ToListAsync();
            return View(settings);
        }

        /// <summary>
        /// Saves updated system configuration settings to the database.
        /// </summary>
        /// <param name="settings">A dictionary of configuration keys and their new values.</param>
        /// <returns>A redirect to the settings index page with a success message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSettings(Dictionary<string, string> settings)
        {
            foreach (var item in settings)
            {
                var setting = await _context.SystemSettings.FirstOrDefaultAsync(s => s.Key == item.Key);
                if (setting != null)
                {
                    setting.Value = item.Value;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Lưu cài đặt hệ thống thành công!";
            return RedirectToAction("Index");
        }
    }
}
