using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly PharmaContext _context;
        private const decimal PointsPerCurrencyUnit = 0.01m; // 1 point for every 100 units spent

        public LoyaltyService(PharmaContext context)
        {
            _context = context;
        }

        public async Task<int> GetPointsAsync(int userId)
        {
            // For now, let's assume we store points in SystemSettings or a future User field
            // Since we don't have a LoyaltyPoints field in User yet, let's mock it
            return 500; // Mock value
        }

        public async Task AddPointsAsync(int userId, decimal orderAmount)
        {
            int pointsToAdd = (int)(orderAmount * PointsPerCurrencyUnit);
            // Logic to update user points in DB
            await Task.CompletedTask;
        }

        public async Task<bool> RedeemPointsAsync(int userId, int pointsToRedeem)
        {
            int currentPoints = await GetPointsAsync(userId);
            if (currentPoints < pointsToRedeem) return false;

            // Logic to deduct points from user
            return true;
        }

        public async Task<decimal> CalculateDiscountFromPointsAsync(int userId)
        {
            int points = await GetPointsAsync(userId);
            return points * 0.1m; // Each point is worth 0.1 units of currency
        }
    }
}
