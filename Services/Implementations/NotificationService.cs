using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Mock email sending
            await Task.Delay(100); // Simulate network latency
            Debug.WriteLine($"[EMAIL SENT] To: {to}, Subject: {subject}");
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            // Mock SMS sending
            await Task.Delay(50);
            Debug.WriteLine($"[SMS SENT] To: {phoneNumber}, Message: {message}");
        }

        public async Task NotifyLowStockAsync(int productId, string productName, int currentStock)
        {
            string message = $"CẢNH BÁO: Sản phẩm {productName} (ID: {productId}) sắp hết hàng. Hiện còn: {currentStock}.";
            await SendEmailAsync("admin@pharmasphere.com", "Cảnh báo hết hàng", message);
        }

        public async Task NotifyOrderConfirmationAsync(int orderId, string customerEmail)
        {
            string body = $"Cảm ơn bạn đã đặt hàng. Mã đơn hàng của bạn là #{orderId}.";
            await SendEmailAsync(customerEmail, "Xác nhận đơn hàng", body);
        }
    }
}
