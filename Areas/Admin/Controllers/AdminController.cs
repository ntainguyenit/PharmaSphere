using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Controller for the Admin dashboard and management features.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AdminController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AdminController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the admin dashboard with key performance indicators and recent activities.
        /// </summary>
        /// <returns>The admin dashboard view.</returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalRevenue = await _context.Orders
                .Where(o => o.Status != Models.OrderStatus.Cancelled)
                .SumAsync(o => o.TotalAmount);
            
            ViewBag.OrderCount = await _context.Orders
                .CountAsync(o => o.OrderDate.Date == DateTime.Today);
            
            ViewBag.LowStockCount = await _context.Products
                .CountAsync(p => p.Stock < 20);
            
            ViewBag.NewCustomerCount = await _context.Users
                .CountAsync(u => u.Role == Models.UserRole.Customer);

            ViewBag.RecentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Include(o => o.Customer)
                .ToListAsync();

            ViewBag.TopSelling = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .Select(g => new { 
                    Product = g.First().Product.Name, 
                    Sold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(x => x.Sold)
                .Take(3)
                .ToListAsync();

            return View();
        }
    }
}
