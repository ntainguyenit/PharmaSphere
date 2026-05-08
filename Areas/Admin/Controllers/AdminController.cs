using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;

namespace PharmaSphere.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AdminController : Controller
    {
        private readonly PharmaContext _context;

        public AdminController(PharmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalRevenue = await _context.Orders.Where(o => o.Status != Models.OrderStatus.Cancelled).SumAsync(o => o.TotalAmount);
            ViewBag.OrderCount = await _context.Orders.CountAsync(o => o.OrderDate.Date == DateTime.Today);
            ViewBag.LowStockCount = await _context.Products.CountAsync(p => p.Stock < 20);
            ViewBag.NewCustomerCount = await _context.Users.CountAsync(u => u.Role == Models.UserRole.Customer);

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
