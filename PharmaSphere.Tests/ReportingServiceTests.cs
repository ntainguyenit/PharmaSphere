using Xunit;
using PharmaSphere.Models;
using PharmaSphere.Services.Implementations;
using PharmaSphere.Services.Interfaces;
using PharmaSphere.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

namespace PharmaSphere.Tests
{
    public class ReportingServiceTests
    {
        [Fact]
        public async Task GetSalesReportAsync_ShouldCalculateCorrectTotals()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PharmaContext>()
                .UseInMemoryDatabase(databaseName: "Test_SalesReport")
                .Options;

            using var context = new PharmaContext(options);
            context.Orders.AddRange(new List<Order>
            {
                new Order { Id = 1, OrderDate = DateTime.Today, TotalAmount = 100, Status = OrderStatus.Delivered },
                new Order { Id = 2, OrderDate = DateTime.Today, TotalAmount = 200, Status = OrderStatus.Pending },
                new Order { Id = 3, OrderDate = DateTime.Today, TotalAmount = 500, Status = OrderStatus.Cancelled }
            });
            await context.SaveChangesAsync();

            var mockInventory = new Mock<IInventoryService>();
            var service = new ReportingService(context, mockInventory.Object);

            // Act
            var result = await service.GetSalesReportAsync(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1));

            // Assert
            Assert.Equal(300, result.TotalRevenue); // 100 + 200 (Cancelled excluded)
            Assert.Equal(2, result.TotalOrders);
        }

        [Fact]
        public async Task GetInventoryStatsAsync_ShouldReturnCorrectStats()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PharmaContext>()
                .UseInMemoryDatabase(databaseName: "Test_InventoryStats")
                .Options;

            using var context = new PharmaContext(options);
            context.Products.AddRange(new List<Product>
            {
                new Product { Id = 1, Price = 10, Stock = 100 },
                new Product { Id = 2, Price = 20, Stock = 0 },
                new Product { Id = 3, Price = 5, Stock = 5 }
            });
            await context.SaveChangesAsync();

            var mockInventory = new Mock<IInventoryService>();
            var service = new ReportingService(context, mockInventory.Object);

            // Act
            var result = await service.GetInventoryStatsAsync();

            // Assert
            Assert.Equal(3, result.TotalProducts);
            Assert.Equal(1, result.OutOfStockItems);
            Assert.Equal(1, result.LowStockItems);
            Assert.Equal(1025, result.TotalInventoryValue); // (10*100) + (20*0) + (5*5) = 1000 + 0 + 25 = 1025
        }
    }
}
