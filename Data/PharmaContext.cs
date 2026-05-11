using Microsoft.EntityFrameworkCore;
using PharmaSphere.Models;

namespace PharmaSphere.Data
{
    /// <summary>
    /// Database context for the PharmaSphere application, managing all entity persistence and database connectivity.
    /// </summary>
    public class PharmaContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PharmaContext"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by this context.</param>
        public PharmaContext(DbContextOptions<PharmaContext> options) : base(options) { }

        /// <summary>Gets or sets the categories table, representing product classifications.</summary>
        public DbSet<Category> Categories { get; set; }
        
        /// <summary>Gets or sets the brands table, representing manufacturers and brands.</summary>
        public DbSet<Brand> Brands { get; set; }
        
        /// <summary>Gets or sets the products table, representing all pharmaceutical items.</summary>
        public DbSet<Product> Products { get; set; }
        
        /// <summary>Gets or sets the users table, containing customers, staff, and administrators.</summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>Gets or sets the orders table, tracking customer transactions.</summary>
        public DbSet<Order> Orders { get; set; }
        
        /// <summary>Gets or sets the order items table, representing individual products within an order.</summary>
        public DbSet<OrderItem> OrderItems { get; set; }
        
        /// <summary>Gets or sets the system settings table, for system-wide configuration data.</summary>
        public DbSet<SystemSetting> SystemSettings { get; set; }

        /// <summary>Gets or sets the audit logs table, for tracking system changes.</summary>
        public DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Configures the model mapping, entity relationships, and precision for decimal types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure decimal precision for financial fields
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // Additional model configurations can be added here
        }
    }
}
