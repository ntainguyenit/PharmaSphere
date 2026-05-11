using Microsoft.Extensions.DependencyInjection;
using PharmaSphere.Services.Interfaces;
using PharmaSphere.Services.Implementations;
using PharmaSphere.Data.Repositories.Interfaces;
using PharmaSphere.Data.Repositories.Implementations;

namespace PharmaSphere.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all business services and repositories in the DI container.
        /// </summary>
        public static IServiceCollection AddPharmaServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            
            // Business Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IInventoryService, InventoryService>();

            return services;
        }
    }
}
