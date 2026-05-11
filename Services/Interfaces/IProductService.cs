using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    /// <summary>
    /// Service for managing products and catalog logic.
    /// </summary>
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string searchTerm, int? categoryId, int page, int pageSize);
        Task<int> GetTotalProductCountAsync(string searchTerm, int? categoryId);
        Task<Product> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Brand>> GetBrandsAsync();
        
        /// <summary>
        /// Complex logic to check if a product needs restocking.
        /// </summary>
        Task<bool> NeedsRestockAsync(int productId);
        
        /// <summary>
        /// Calculates the discounted price based on various business rules.
        /// </summary>
        decimal CalculateDiscountedPrice(Product product, User user);
    }
}
