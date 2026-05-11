using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IReportingService
    {
        Task<SalesReport> GetSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<InventoryStats> GetInventoryStatsAsync();
        Task<List<TopProduct>> GetTopSellingProductsAsync(int count = 5);
        Task<DashboardSummary> GetDashboardSummaryAsync();
    }

    public class SalesReport
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public Dictionary<DateTime, decimal> DailyRevenue { get; set; }
    }

    public class InventoryStats
    {
        public int TotalProducts { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public decimal TotalInventoryValue { get; set; }
    }

    public class TopProduct
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int UnitsSold { get; set; }
        public decimal RevenueGenerated { get; set; }
    }

    public class DashboardSummary
    {
        public decimal TodayRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int NewCustomers { get; set; }
        public List<InventoryAlert> RecentAlerts { get; set; }
    }
}
