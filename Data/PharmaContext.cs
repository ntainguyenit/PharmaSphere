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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Suppress the PendingModelChangesWarning which is very strict in EF Core 9
            optionsBuilder.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }

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

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

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

            modelBuilder.Entity<Coupon>()
                .Property(c => c.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Coupon>()
                .Property(c => c.DiscountPercentage)
                .HasPrecision(18, 2);

            // Additional model configurations can be added here
        }

        /// <summary>
        /// Overrides SaveChangesAsync to automatically capture audit logs for all changes.
        /// </summary>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(auditEntries);
            return result;
        }

        private List<AuditEntry> OnBeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }

        private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return Task.CompletedTask;

            foreach (var auditEntry in auditEntries)
            {
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                AuditLogs.Add(auditEntry.ToAudit());
            }
            return SaveChangesAsync();
        }
    }

    /// <summary>
    /// Helper class to manage audit entry data during the SaveChanges process.
    /// </summary>
    internal class AuditEntry
    {
        public AuditEntry(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry)
        {
            Entry = entry;
        }
        public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry { get; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry> TemporaryProperties { get; } = new List<Microsoft.EntityFrameworkCore.ChangeTracking.PropertyEntry>();
        public bool HasTemporaryProperties => TemporaryProperties.Any();

        public AuditLog ToAudit()
        {
            var audit = new AuditLog();
            audit.EntityName = TableName;
            audit.Timestamp = DateTime.Now;
            audit.EntityId = System.Text.Json.JsonSerializer.Serialize(KeyValues);
            audit.Action = Entry.State.ToString();
            audit.Changes = System.Text.Json.JsonSerializer.Serialize(new { OldValues, NewValues });
            return audit;
        }
    }
}
