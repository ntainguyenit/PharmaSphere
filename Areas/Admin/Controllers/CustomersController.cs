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
    /// Controller for managing customers in the admin area.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class CustomersController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CustomersController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a paginated list of customers with search functionality.
        /// </summary>
        /// <param name="searchTerm">The search term to filter customers by name or email.</param>
        /// <param name="page">The current page number.</param>
        /// <returns>The customer list view.</returns>
        public async Task<IActionResult> Index(string searchTerm, int page = 1)
        {
            int pageSize = 10;
            var query = _context.Users
                .Where(u => u.Role == UserRole.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(u => u.FullName.Contains(searchTerm) || 
                                       u.Email.Contains(searchTerm) || 
                                       u.Username.Contains(searchTerm));
            }

            var totalItems = await query.CountAsync();
            var customers = await query.OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewData["Title"] = "Quản lý khách hàng";

            return View(customers);
        }

        /// <summary>
        /// Deletes a customer from the system.
        /// </summary>
        /// <param name="id">The customer ID.</param>
        /// <returns>A redirect to the Index view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role == UserRole.Customer)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa khách hàng thành công.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
