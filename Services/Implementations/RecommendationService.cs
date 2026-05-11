using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;
using PharmaSphere.Data;
using Microsoft.EntityFrameworkCore;

namespace PharmaSphere.Services.Implementations
{
    public class RecommendationService : IRecommendationService
    {
        private readonly PharmaContext _context;

        public RecommendationService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetPersonalizedRecommendationsAsync(int userId, int count = 5)
        {
            // Complex mock logic: Find what user bought, find others who bought the same, recommend what they also bought.
            var userOrders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.CustomerId == userId)
                .ToListAsync();

            var purchasedProductIds = userOrders.SelectMany(o => o.Items.Select(i => i.ProductId)).Distinct().ToList();

            if (!purchasedProductIds.Any())
            {
                return await GetTrendingProductsAsync(count);
            }

            // Mocking the 'Similar Users' logic
            var recommendations = await _context.Products
                .Where(p => !purchasedProductIds.Contains(p.Id) && p.Status == ProductStatus.Active)
                .OrderBy(p => Guid.NewGuid()) // Random for mock
                .Take(count)
                .ToListAsync();

            return recommendations;
        }

        public async Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 5)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return new List<Product>();

            return await _context.Products
                .Where(p => p.Id != productId && p.CategoryId == product.CategoryId && p.Status == ProductStatus.Active)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Product>> GetTrendingProductsAsync(int count = 10)
        {
            // Logic based on recent high-volume orders
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            
            var trendingIds = await _context.OrderItems
                .Where(i => i.Order.OrderDate >= thirtyDaysAgo)
                .GroupBy(i => i.ProductId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(count)
                .ToListAsync();

            return await _context.Products
                .Where(p => trendingIds.Contains(p.Id))
                .ToListAsync();
        }
    }
}
