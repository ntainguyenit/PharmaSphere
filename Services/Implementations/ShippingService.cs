using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class ShippingService : IShippingService
    {
        public async Task<List<ShippingOption>> GetShippingOptionsAsync(string address)
        {
            await Task.Delay(100); // Simulate API call
            
            return new List<ShippingOption>
            {
                new ShippingOption { Provider = "Giao Hàng Nhanh", Method = "Standard", Cost = 25000, EstimatedDelivery = "2-3 ngày" },
                new ShippingOption { Provider = "GrabExpress", Method = "Instant", Cost = 50000, EstimatedDelivery = "1-2 giờ" },
                new ShippingOption { Provider = "VNPost", Method = "Economy", Cost = 15000, EstimatedDelivery = "4-5 ngày" }
            };
        }

        public async Task<decimal> CalculateShippingCostAsync(int orderId, string provider)
        {
            // Logic to calculate cost based on weight and distance
            return await Task.FromResult(provider switch
            {
                "GrabExpress" => 50000,
                "Giao Hàng Nhanh" => 25000,
                _ => 15000
            });
        }

        public async Task<string> GenerateTrackingNumberAsync(int orderId)
        {
            return await Task.FromResult("PS" + DateTime.Now.ToString("yyyyMMdd") + orderId.ToString().PadLeft(5, '0'));
        }

        public async Task<ShippingLabel> CreateShippingLabelAsync(int orderId)
        {
            return await Task.FromResult(new ShippingLabel
            {
                TrackingNumber = await GenerateTrackingNumberAsync(orderId),
                Barcode = Guid.NewGuid().ToString("N").ToUpper(),
                LabelPdf = new byte[0] // Mock data
            });
        }
    }
}
