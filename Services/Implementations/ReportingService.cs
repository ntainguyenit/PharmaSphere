using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class ReportingService : IReportingService
    {
        private readonly PharmaContext _context;
        private readonly IInventoryService _inventoryService;

        public ReportingService(PharmaContext context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public async Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.Status != OrderStatus.Cancelled)
                .ToListAsync();

            var report = new SalesReport
            {
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                TotalOrders = orders.Count,
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                DailyRevenue = orders.GroupBy(o => o.OrderDate.Date)
                                    .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount))
            };

            return report;
        }

        public async Task<InventoryStats> GetInventoryStatsAsync()
        {
            var products = await _context.Products.ToListAsync();
            
            return new InventoryStats
            {
                TotalProducts = products.Count,
                LowStockItems = products.Count(p => p.Stock > 0 && p.Stock <= 10),
                OutOfStockItems = products.Count(p => p.Stock == 0),
                TotalInventoryValue = products.Sum(p => p.Price * p.Stock)
            };
        }

        public async Task<List<TopProduct>> GetTopSellingProductsAsync(int count = 5)
        {
            var topProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new TopProduct
                {
                    ProductId = g.Key.ProductId,
                    Name = g.Key.Name,
                    UnitsSold = g.Sum(oi => oi.Quantity),
                    RevenueGenerated = g.Sum(oi => oi.Price * oi.Quantity)
                })
                .OrderByDescending(p => p.UnitsSold)
                .Take(count)
                .ToListAsync();

            return topProducts;
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var today = DateTime.Today;
            var todayRevenue = await _context.Orders
                .Where(o => o.OrderDate >= today && o.Status != OrderStatus.Cancelled)
                .SumAsync(o => o.TotalAmount);

            var pendingOrders = await _context.Orders
                .CountAsync(o => o.Status == OrderStatus.Pending);

            var newCustomers = await _context.Users
                .CountAsync(u => u.Role == UserRole.Customer); // Ideally we'd have a CreatedAt date

            var alerts = await _inventoryService.GetInventoryAlertsAsync();

            return new DashboardSummary
            {
                TodayRevenue = todayRevenue,
                PendingOrders = pendingOrders,
                NewCustomers = newCustomers,
                RecentAlerts = alerts.Take(5).ToList()
            };
        }
    }
}
