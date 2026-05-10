using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing employees in the admin area. Restricted to Administrators.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth", Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public EmployeesController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a paginated list of employees with search functionality.
        /// </summary>
        /// <param name="searchTerm">The search term to filter employees by name, email or role.</param>
        /// <param name="page">The current page number.</param>
        /// <returns>The employee list view.</returns>
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;
            var query = _context.Users
                .Where(u => u.Role == UserRole.Admin || u.Role == UserRole.Staff)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm) || 
                                       u.Email.Contains(searchTerm) || 
                                       u.Username.Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var employees = await query.OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewData["Title"] = "Quản lý nhân viên";

            return View(employees);
        }

        /// <summary>
        /// Deletes an employee from the system.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>A redirect to the Index view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && (user.Role == UserRole.Admin || user.Role == UserRole.Staff))
            {
                // Prevent self-deletion if current user
                if (user.Username == User.Identity.Name)
                {
                    TempData["Error"] = "Bạn không thể tự xóa tài khoản của chính mình.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa nhân viên thành công.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
