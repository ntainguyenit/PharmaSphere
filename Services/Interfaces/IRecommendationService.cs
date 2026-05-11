using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaSphere.Models;
using PharmaSphere.Data.Repositories.Interfaces;

namespace PharmaSphere.Services.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<Product>> GetPersonalizedRecommendationsAsync(int userId, int count = 5);
        Task<List<Product>> GetRelatedProductsAsync(int productId, int count = 5);
        Task<List<Product>> GetTrendingProductsAsync(int count = 10);
    }
}
