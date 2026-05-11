using Xunit;
using PharmaSphere.Models;
using PharmaSphere.Services.Implementations;
using System.Collections.Generic;
using System.Linq;

namespace PharmaSphere.Tests
{
    public class MappingServiceTests
    {
        private readonly MappingService _mappingService;

        public MappingServiceTests()
        {
            _mappingService = new MappingService();
        }

        [Fact]
        public void ToProductDTO_ShouldMapCorrectly()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Paracetamol",
                Price = 5000,
                Stock = 50,
                Category = new Category { Name = "Painkiller" },
                Brand = new Brand { Name = "Panadol" }
            };

            // Act
            var result = _mappingService.ToProductDTO(product);

            // Assert
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal("Painkiller", result.CategoryName);
            Assert.Equal("Panadol", result.BrandName);
            Assert.False(result.IsLowStock);
        }

        [Fact]
        public void ToProductDTO_ShouldDetectLowStock()
        {
            // Arrange
            var product = new Product { Stock = 5 };

            // Act
            var result = _mappingService.ToProductDTO(product);

            // Assert
            Assert.True(result.IsLowStock);
        }
    }
}
