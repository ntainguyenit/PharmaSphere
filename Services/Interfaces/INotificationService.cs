using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmaSphere.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendSmsAsync(string phoneNumber, string message);
        Task NotifyLowStockAsync(int productId, string productName, int currentStock);
        Task NotifyOrderConfirmationAsync(int orderId, string customerEmail);
    }
}
