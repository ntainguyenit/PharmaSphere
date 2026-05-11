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
    public class OrderService : IOrderService
    {
        private readonly PharmaContext _context;
        private readonly IAuditService _auditService;

        public OrderService(PharmaContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order> CreateOrderAsync(int customerId, string address, string phone, string paymentMethod, List<OrderItem> items)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    CustomerId = customerId,
                    Address = address,
                    Phone = phone,
                    PaymentMethod = paymentMethod,
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.Pending,
                    TotalAmount = await CalculateTotalAsync(items)
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                foreach (var item in items)
                {
                    item.OrderId = order.Id;
                    _context.OrderItems.Add(item);

                    // Update stock
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock -= item.Quantity;
                        if (product.Stock < 0) throw new Exception($"Sản phẩm {product.Name} đã hết hàng.");
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                await _auditService.LogAsync("Order", order.Id.ToString(), "Create", $"Order created with total {order.TotalAmount}", customerId.ToString());

                return order;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return false;

            var oldStatus = order.Status;
            order.Status = status;
            
            bool success = await _context.SaveChangesAsync() > 0;
            if (success)
            {
                await _auditService.LogAsync("Order", orderId.ToString(), "UpdateStatus", $"Status changed from {oldStatus} to {status}", "System");
            }
            return success;
        }

        public async Task<decimal> CalculateTotalAsync(List<OrderItem> items)
        {
            decimal total = 0;
            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    total += product.Price * item.Quantity;
                }
            }
            return total;
        }

        public async Task<(bool IsValid, string Message)> ValidateOrderAsync(int customerId, List<OrderItem> items)
        {
            if (items == null || !items.Any())
                return (false, "Giỏ hàng trống.");

            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return (false, $"Sản phẩm ID {item.ProductId} không tồn tại.");
                
                if (product.Stock < item.Quantity)
                    return (false, $"Sản phẩm {product.Name} không đủ tồn kho.");

                if (product.IsPrescription)
                {
                    // Mock check for prescription
                    // In a real app, we would check if a valid prescription is uploaded
                    return (false, $"Sản phẩm {product.Name} yêu cầu đơn thuốc của bác sĩ.");
                }
            }

            return (true, "Hợp lệ.");
        }
    }
}
