using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10);
        Task<IEnumerable<Product>> GetExpiringProductsAsync(int daysThreshold = 30);
        Task<bool> RestockProductAsync(int productId, int quantity, string batchNumber, DateTime expiryDate);
        Task<IEnumerable<InventoryAlert>> GetInventoryAlertsAsync();
    }

    public class InventoryAlert
    {
        public string Type { get; set; } // LowStock, Expiring
        public string Message { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
