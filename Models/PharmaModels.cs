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
}
