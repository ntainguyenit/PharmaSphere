using System;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    /// <summary>
    /// Service to manage customer loyalty points and rewards.
    /// </summary>
    public interface ILoyaltyService
    {
        Task<int> GetPointsAsync(int userId);
        Task AddPointsAsync(int userId, decimal orderAmount);
        Task<bool> RedeemPointsAsync(int userId, int pointsToRedeem);
        Task<decimal> CalculateDiscountFromPointsAsync(int userId);
    }
}
