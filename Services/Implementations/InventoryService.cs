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
    public class InventoryService : IInventoryService
    {
        private readonly PharmaContext _context;
        private readonly IAuditService _auditService;

        public InventoryService(PharmaContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
        {
            return await _context.Products
                .Where(p => p.Stock <= threshold)
                .OrderBy(p => p.Stock)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetExpiringProductsAsync(int daysThreshold = 30)
        {
            var targetDate = DateTime.Now.AddDays(daysThreshold);
            return await _context.Products
                .Where(p => p.ExpiryDate != null && p.ExpiryDate <= targetDate)
                .OrderBy(p => p.ExpiryDate)
                .ToListAsync();
        }

        public async Task<bool> RestockProductAsync(int productId, int quantity, string batchNumber, DateTime expiryDate)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            int oldStock = product.Stock;
            product.Stock += quantity;
            product.BatchNumber = batchNumber;
            product.ExpiryDate = expiryDate;
            product.Status = ProductStatus.InStock;

            bool success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _auditService.LogAsync("Inventory", productId.ToString(), "Restock", 
                    $"Restocked {quantity} units. New stock: {product.Stock}. Batch: {batchNumber}, Expiry: {expiryDate.ToShortDateString()}", 
                    "System");
            }
            return success;
        }

        public async Task<IEnumerable<InventoryAlert>> GetInventoryAlertsAsync()
        {
            var alerts = new List<InventoryAlert>();

            // Check for low stock
            var lowStock = await GetLowStockProductsAsync();
            foreach (var p in lowStock)
            {
                alerts.Add(new InventoryAlert
                {
                    Type = "LowStock",
                    Message = $"Sản phẩm {p.Name} sắp hết hàng ({p.Stock} còn lại).",
                    ProductId = p.Id,
                    ProductName = p.Name
                });
            }

            // Check for expiring products
            var expiring = await GetExpiringProductsAsync();
            foreach (var p in expiring)
            {
                alerts.Add(new InventoryAlert
                {
                    Type = "Expiring",
                    Message = $"Sản phẩm {p.Name} sắp hết hạn ({p.ExpiryDate?.ToShortDateString()}).",
                    ProductId = p.Id,
                    ProductName = p.Name
                });
            }

            return alerts;
        }
    }
}
