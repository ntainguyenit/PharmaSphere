using System.Collections.Generic;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(int id);
        Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> CreateOrderAsync(int customerId, string address, string phone, string paymentMethod, List<OrderItem> items);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<decimal> CalculateTotalAsync(List<OrderItem> items);
        
        /// <summary>
        /// Validates an order before placement.
        /// </summary>
        Task<(bool IsValid, string Message)> ValidateOrderAsync(int customerId, List<OrderItem> items);
    }
}
