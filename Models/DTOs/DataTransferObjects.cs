using System;
using System.Collections.Generic;
using PharmaSphere.Models;

namespace PharmaSphere.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string FormattedPrice { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public bool IsLowStock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrescriptionRequired { get; set; }
    }

    public class OrderSummaryDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public int ItemCount { get; set; }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
