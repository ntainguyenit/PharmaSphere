using Xunit;
using Moq;
using PharmaSphere.Models;
using PharmaSphere.Services.Implementations;
using PharmaSphere.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaSphere.Tests
{
    public class ProductServiceTests
    {
        [Fact]
        public void CalculateDiscountedPrice_ShouldApplyBulkDiscount()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PharmaContext>()
                .UseInMemoryDatabase(databaseName: "Test_BulkDiscount")
                .Options;

            using var context = new PharmaContext(options);
            var service = new ProductService(context);
            var product = new Product { Price = 100, Stock = 150 }; // Over 100 triggers 5% discount

            // Act
            var result = service.CalculateDiscountedPrice(product, null);

            // Assert
            Assert.Equal(95, result);
        }

        [Fact]
        public void CalculateDiscountedPrice_ShouldApplyCategoryPromotion()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PharmaContext>()
                .UseInMemoryDatabase(databaseName: "Test_CategoryPromotion")
                .Options;

            using var context = new PharmaContext(options);
            var service = new ProductService(context);
            var product = new Product 
            { 
                Price = 100, 
                Category = new Category { Name = "Vitamin" } 
            }; // Vitamin triggers 10% discount

            // Act
            var result = service.CalculateDiscountedPrice(product, null);

            // Assert
            Assert.Equal(90, result);
        }

        [Fact]
        public async Task NeedsRestockAsync_ShouldReturnTrue_WhenStockIsLow()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PharmaContext>()
                .UseInMemoryDatabase(databaseName: "Test_Restock")
                .Options;

            using var context = new PharmaContext(options);
            var product = new Product { Id = 99, Stock = 5 };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var service = new ProductService(context);

            // Act
            var result = await service.NeedsRestockAsync(99);

            // Assert
            Assert.True(result);
        }
    }
}
