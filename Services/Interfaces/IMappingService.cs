using System;
using System.Collections.Generic;
using System.Linq;
using PharmaSphere.Models;
using PharmaSphere.Models.DTOs;
using PharmaSphere.Common.Helpers;

namespace PharmaSphere.Services.Interfaces
{
    public interface IMappingService
    {
        ProductDTO ToProductDTO(Product product);
        OrderSummaryDTO ToOrderSummaryDTO(Order order);
        UserDTO ToUserDTO(User user);
        IEnumerable<ProductDTO> ToProductDTOs(IEnumerable<Product> products);
    }
}
