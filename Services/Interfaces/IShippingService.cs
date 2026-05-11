using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmaSphere.Services.Interfaces
{
    public interface IShippingService
    {
        Task<List<ShippingOption>> GetShippingOptionsAsync(string address);
        Task<decimal> CalculateShippingCostAsync(int orderId, string provider);
        Task<string> GenerateTrackingNumberAsync(int orderId);
        Task<ShippingLabel> CreateShippingLabelAsync(int orderId);
    }

    public class ShippingOption
    {
        public string Provider { get; set; }
        public string Method { get; set; }
        public decimal Cost { get; set; }
        public string EstimatedDelivery { get; set; }
    }

    public class ShippingLabel
    {
        public string TrackingNumber { get; set; }
        public string Barcode { get; set; }
        public byte[] LabelPdf { get; set; }
    }
}
