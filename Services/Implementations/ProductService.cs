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
    public class ProductService : IProductService
    {
        private readonly PharmaContext _context;

        public ProductService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string searchTerm, int? categoryId, int page, int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || 
                                         p.Description.ToLower().Contains(searchTerm) ||
                                         p.Ingredients.ToLower().Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalProductCountAsync(string searchTerm, int? categoryId)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm) || 
                                         p.Description.ToLower().Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            return await query.CountAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            try 
            {
                _context.Products.Add(product);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // In a real app, we would log this
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<bool> NeedsRestockAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) return false;

            // Business logic: Products with stock less than 10 need restocking
            // This could be more complex, checking sales velocity
            return product.Stock < 10;
        }

        public decimal CalculateDiscountedPrice(Product product, User user)
        {
            decimal price = product.Price;

            // Business Rule 1: Bulk discount
            if (product.Stock > 100) 
            {
                price *= 0.95m; // 5% discount for overstocked items
            }

            // Business Rule 2: Membership discount
            if (user != null)
            {
                if (user.Role == UserRole.Customer)
                {
                    price *= 0.98m; // 2% loyalty discount
                }
            }

            // Business Rule 3: Category specific promotion
            if (product.Category?.Name == "Vitamin")
            {
                price *= 0.90m; // 10% off Vitamins
            }

            return Math.Round(price, 2);
        }
    }
}
