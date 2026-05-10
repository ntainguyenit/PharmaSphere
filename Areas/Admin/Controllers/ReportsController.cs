using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PharmaSphere.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller responsible for generating and displaying business performance reports and statistics. Restricted to Administrators.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth", Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReportsController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Compiles business metrics including revenue trends, order distribution, and top products for display on the dashboard.
        /// </summary>
        /// <returns>A view populated with various statistical data points.</returns>
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Thống kê báo cáo";

            // 1. Revenue last 7 days
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .OrderBy(d => d)
                .ToList();

            var revenueData = await _context.Orders
                .Where(o => o.OrderDate >= last7Days.First() && o.Status != Models.OrderStatus.Cancelled)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.TotalAmount) })
                .ToListAsync();

            ViewBag.RevenueLabels = last7Days.Select(d => d.ToString("dd/MM")).ToList();
            ViewBag.RevenueValues = last7Days.Select(d => revenueData.FirstOrDefault(r => r.Date == d)?.Revenue ?? 0).ToList();

            // 2. Orders status distribution
            var statusCounts = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();
            
            ViewBag.StatusLabels = statusCounts.Select(s => s.Status).ToList();
            ViewBag.StatusValues = statusCounts.Select(s => s.Count).ToList();

            // 3. Category distribution
            var categoryStats = await _context.OrderItems
                .Include(oi => oi.Product).ThenInclude(p => p.Category)
                .GroupBy(oi => oi.Product.Category.Name)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToListAsync();

            ViewBag.CategoryLabels = categoryStats.Select(c => c.Category).ToList();
            ViewBag.CategoryValues = categoryStats.Select(c => c.Count).ToList();

            // 4. Top products
            ViewBag.TopProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new { 
                    Name = g.First().Product.Name, 
                    Sold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Quantity * oi.Price)
                })
                .OrderByDescending(x => x.Revenue)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}
