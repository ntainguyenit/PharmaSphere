using System;
using System.Collections.Generic;
using System.Linq;
using PharmaSphere.Models;
using PharmaSphere.Models.DTOs;
using PharmaSphere.Common.Helpers;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class MappingService : IMappingService
    {
        public ProductDTO ToProductDTO(Product product)
        {
            if (product == null) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                FormattedPrice = CurrencyFormatter.ToVND(product.Price),
                Stock = product.Stock,
                CategoryName = product.Category?.Name ?? "Unknown",
                BrandName = product.Brand?.Name ?? "Unknown",
                IsLowStock = product.Stock <= 10,
                ImageUrl = product.ImageUrl,
                IsPrescriptionRequired = product.IsPrescription
            };
        }

        public OrderSummaryDTO ToOrderSummaryDTO(Order order)
        {
            if (order == null) return null;

            return new OrderSummaryDTO
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                CustomerName = order.Customer?.FullName ?? "Unknown",
                ItemCount = order.Items?.Count ?? 0
            };
        }

        public UserDTO ToUserDTO(User user)
        {
            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public IEnumerable<ProductDTO> ToProductDTOs(IEnumerable<Product> products)
        {
            return products?.Select(ToProductDTO) ?? Enumerable.Empty<ProductDTO>();
        }
    }
}
