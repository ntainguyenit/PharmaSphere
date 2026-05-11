using System.Collections.Generic;

namespace PharmaSphere.Models
{
    /// <summary>
    /// Represents a product category.
    /// </summary>
    public class Category
    {
        /// <summary>Gets or sets the category ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the category name.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets the Lucide icon name for this category.</summary>
        public string Icon { get; set; }
        /// <summary>Gets or sets the collection of products in this category.</summary>
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Represents a product brand.
    /// </summary>
    public class Brand
    {
        /// <summary>Gets or sets the brand ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the brand name.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets the collection of products belonging to this brand.</summary>
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Represents a pharmaceutical product.
    /// </summary>
    public class Product
    {
        /// <summary>Gets or sets the product ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the product name.</summary>
        public string Name { get; set; }
        /// <summary>Gets or sets the product description.</summary>
        public string Description { get; set; }
        /// <summary>Gets or sets the product ingredients.</summary>
        public string Ingredients { get; set; }
        /// <summary>Gets or sets the product price.</summary>
        public decimal Price { get; set; }
        /// <summary>Gets or sets the stock quantity.</summary>
        public int Stock { get; set; }
        /// <summary>Gets or sets the image URL.</summary>
        public string ImageUrl { get; set; }
        /// <summary>Gets or sets a value indicating whether this product requires a prescription.</summary>
        public bool IsPrescription { get; set; }
        /// <summary>Gets or sets the product rating (0-5).</summary>
        public double Rating { get; set; } = 5.0;
        /// <summary>Gets or sets the number of reviews.</summary>
        public int ReviewCount { get; set; } = 0;

        /// <summary>Gets or sets the category ID.</summary>
        public int CategoryId { get; set; }
        /// <summary>Gets or sets the category navigation property.</summary>
        public virtual Category Category { get; set; }

        /// <summary>Gets or sets the brand ID.</summary>
        public int BrandId { get; set; }
        /// <summary>Gets or sets the brand navigation property.</summary>
        public virtual Brand Brand { get; set; }

        /// <summary>Gets or sets the product status.</summary>
        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        /// <summary>Gets or sets the expiration date of the current batch.</summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>Gets or sets the batch number.</summary>
        public string BatchNumber { get; set; }
    }

    /// <summary>
    /// Defines product statuses.
    /// </summary>
    public enum ProductStatus
    {
        /// <summary>Product is in stock and available.</summary>
        InStock,
        /// <summary>Product is out of stock.</summary>
        OutOfStock,
        /// <summary>Product is coming soon.</summary>
        ComingSoon,
        /// <summary>Product is discontinued.</summary>
        Discontinued,
        /// <summary>Product is on promotion.</summary>
        OnSale
    }

    /// <summary>
    /// Defines user roles in the system.
    /// </summary>
    public enum UserRole
    {
        /// <summary>Administrator with full access.</summary>
        Admin,
        /// <summary>Staff member with limited management access.</summary>
        Staff,
        /// <summary>Customer with shopping access.</summary>
        Customer
    }

    /// <summary>
    /// Represents a user of the system.
    /// </summary>
    public class User
    {
        /// <summary>Gets or sets the user ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the username.</summary>
        public string Username { get; set; }
        /// <summary>Gets or sets the hashed password.</summary>
        public string PasswordHash { get; set; }
        /// <summary>Gets or sets the full name.</summary>
        public string FullName { get; set; }
        /// <summary>Gets or sets the email address.</summary>
        public string Email { get; set; }
        /// <summary>Gets or sets the phone number.</summary>
        public string Phone { get; set; }
        /// <summary>Gets or sets the user role.</summary>
        public UserRole Role { get; set; }
        /// <summary>Gets or sets the collection of orders placed by this user.</summary>
        public virtual ICollection<Order> Orders { get; set; }
    }

    /// <summary>
    /// Defines the possible states of an order.
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>Order has been placed but not yet processed.</summary>
        Pending,
        /// <summary>Order has been confirmed by staff.</summary>
        Confirmed,
        /// <summary>Order is being shipped.</summary>
        Shipping,
        /// <summary>Order has been delivered to the customer.</summary>
        Delivered,
        /// <summary>Order has been cancelled.</summary>
        Cancelled
    }

    /// <summary>
    /// Represents a customer order.
    /// </summary>
    public class Order
    {
        /// <summary>Gets or sets the order ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the date the order was placed.</summary>
        public DateTime OrderDate { get; set; } = DateTime.Now;
        /// <summary>Gets or sets the total amount of the order.</summary>
        public decimal TotalAmount { get; set; }
        /// <summary>Gets or sets the order status.</summary>
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        /// <summary>Gets or sets the shipping address.</summary>
        public string Address { get; set; }
        /// <summary>Gets or sets the contact phone number for this order.</summary>
        public string Phone { get; set; }
        /// <summary>Gets or sets the payment method used.</summary>
        public string PaymentMethod { get; set; }
        
        /// <summary>Gets or sets the customer ID.</summary>
        public int CustomerId { get; set; }
        /// <summary>Gets or sets the customer navigation property.</summary>
        public virtual User Customer { get; set; }

        /// <summary>Gets or sets the collection of items in this order.</summary>
        public virtual ICollection<OrderItem> Items { get; set; }
    }

    /// <summary>
    /// Represents an individual item within an order.
    /// </summary>
    public class OrderItem
    {
        /// <summary>Gets or sets the order item ID.</summary>
        public int Id { get; set; }
        /// <summary>Gets or sets the order ID.</summary>
        public int OrderId { get; set; }
        /// <summary>Gets or sets the order navigation property.</summary>
        public virtual Order Order { get; set; }

        /// <summary>Gets or sets the product ID.</summary>
        public int ProductId { get; set; }
        /// <summary>Gets or sets the product navigation property.</summary>
        public virtual Product Product { get; set; }

        /// <summary>Gets or sets the quantity ordered.</summary>
        public int Quantity { get; set; }
        /// <summary>Gets or sets the price of the product at the time of order.</summary>
        public decimal Price { get; set; }
    }

    /// <summary>
    /// Represents a system-wide setting.
    /// </summary>
    public class SystemSetting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Represents an audit log entry for tracking system changes.
    /// </summary>
    public class AuditLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string Action { get; set; } // Create, Update, Delete
        public string Changes { get; set; } // JSON or text representation of changes
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }

    /// <summary>
    /// Represents a pharmaceutical supplier.
    /// </summary>
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Represents a drug manufacturer.
    /// </summary>
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Website { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }

    /// <summary>
    /// Represents a customer review for a product.
    /// </summary>
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Represents a discount coupon.
    /// </summary>
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
    }

    /// <summary>
    /// Represents a medical prescription.
    /// </summary>
    public class Prescription
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string DoctorLicenseNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string ImageUrl { get; set; }
        public bool IsVerified { get; set; } = false;
        public int? VerifiedById { get; set; }
        public virtual User VerifiedBy { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
