using Microsoft.EntityFrameworkCore;
using PharmaSphere.Models;

namespace PharmaSphere.Data
{
    /// <summary>
    /// Database context for the PharmaSphere application, managing all entity persistence.
    /// </summary>
    public class PharmaContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PharmaContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by this context.</param>
        public PharmaContext(DbContextOptions<PharmaContext> options) : base(options) { }

        /// <summary>Gets or sets the categories table.</summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>Gets or sets the brands table.</summary>
        public DbSet<Brand> Brands { get; set; }
        /// <summary>Gets or sets the products table.</summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>Gets or sets the users table.</summary>
        public DbSet<User> Users { get; set; }
        /// <summary>Gets or sets the orders table.</summary>
        public DbSet<Order> Orders { get; set; }
        /// <summary>Gets or sets the order items table.</summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>
        /// Configures the model mapping and entity relationships.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            // Seed initial data or configurations
        }
    }
}
