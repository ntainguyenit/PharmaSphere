using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.IntegrationTests
{
    public class BusinessLogicIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public BusinessLogicIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OrderFlow_ShouldCreateOrderAndLogAudit()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PharmaContext>();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

            // Ensure we have a product
            var product = new Product { Name = "Test Product", Price = 100, Stock = 50, CategoryId = 1, BrandId = 1 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var items = new List<OrderItem>
            {
                new OrderItem { ProductId = product.Id, Quantity = 2, Price = 100 }
            };

            // Act
            var order = await orderService.CreateOrderAsync(1, "Test Address", "0123456789", "COD", items);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(200, order.TotalAmount);

            // Verify Audit Log was created automatically by DbContext
            var auditLog = context.AuditLogs.FirstOrDefault(l => l.EntityName == "Order" && l.Action == "Added");
            Assert.NotNull(auditLog);
            
            // Verify Stock was reduced
            var updatedProduct = await context.Products.FindAsync(product.Id);
            Assert.Equal(48, updatedProduct.Stock);
        }

        [Fact]
        public async Task DiscountEngine_ShouldIntegrateWithOrderService()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<PharmaContext>();
            var discountEngine = scope.ServiceProvider.GetRequiredService<IDiscountEngine>();

            var order = new Order { TotalAmount = 2000000 }; // 2M triggers discount
            var user = new User { Role = UserRole.Customer };

            // Act
            var finalPrice = discountEngine.ApplyDiscounts(order, user);

            // Assert
            Assert.True(finalPrice < 2000000);
        }
    }
}
