using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PharmaSphere.Data;
using PharmaSphere.Models;
using System.Collections.Generic;
using Bogus;
using PharmaSphere;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "CustomerAuth";
    })
    .AddCookie("CustomerAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "PharmaSphere_Customer";
    })
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
        options.Cookie.Name = "PharmaSphere_Admin";
    });

builder.Services.AddDbContext<PharmaSphere.Data.PharmaContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Build the application
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Database seeding and dynamic encoding/naming fix for Categories
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PharmaContext>();
    
    // Fix Category names
    var names = new[] { "Thuốc cảm cúm", "Vitamin", "Tim mạch", "Tiểu đường", "Thiết bị y tế", "Chăm sóc cá nhân", "Dược mỹ phẩm", "Mẹ và Bé", "Thực phẩm chức năng", "Hỗ trợ tiêu hóa" };
    var cats = context.Categories.OrderBy(c => c.Id).ToList();
    for (int i = 0; i < Math.Min(cats.Count, names.Length); i++) 
    {
        cats[i].Name = names[i];
    }

    // Seed Users (Realistic Vietnamese data)
    DbInitializer.Seed(scope.ServiceProvider);
    // Seed Products (200 sample products with correct Vietnamese names)
    if (!context.Products.Any(p => p.Name.Contains("Sản phẩm")))
    {
        // Clear existing products to fix encoding issues
        context.Database.ExecuteSqlRaw("DELETE FROM OrderItems; DELETE FROM Products; DBCC CHECKIDENT ('Products', RESEED, 0);");
        
        // Ensure at least one Brand exists as it's required
        if (!context.Brands.Any())
        {
            context.Brands.Add(new Brand { Name = "DHG Pharma" });
            context.Brands.Add(new Brand { Name = "Traphaco" });
            context.Brands.Add(new Brand { Name = "OPC" });
            context.Brands.Add(new Brand { Name = "Nam Ha Pharma" });
            context.SaveChanges();
        }

        var productsToSeed = new List<Product>();
        var brands = context.Brands.ToList();
        var allCats = context.Categories.ToList();

        if (allCats.Any() && brands.Any())
        {
            for (int i = 1; i <= 200; i++)
            {
                var cat = allCats[i % allCats.Count];
                var brand = brands[i % brands.Count];
                productsToSeed.Add(new Product
                {
                    Name = $"Sản phẩm thuốc mẫu số {i}",
                    Description = $"Mô tả chi tiết cho sản phẩm thuốc mẫu số {i}. Hỗ trợ điều trị và tăng cường sức khỏe.",
                    Price = 50000 + (i * 1000),
                    Stock = 50 + i,
                    CategoryId = cat.Id,
                    BrandId = brand.Id,
                    ImageUrl = $"https://picsum.photos/seed/product{i}/400/400",
                    Status = (ProductStatus)(i % 5) // Cycle through InStock, OutOfStock, ComingSoon, Discontinued, OnSale
                });
            }
            context.Products.AddRange(productsToSeed);
            context.SaveChanges();
        }
    }
    
    context.SaveChanges();

    // Seed Orders (Realistic data for 100 orders)
    if (!context.Orders.Any())
    {
        var faker = new Faker("vi"); // Use Vietnamese locale
        var customers = context.Users.Where(u => u.Role == UserRole.Customer).ToList();
        var products = context.Products.ToList();
        var ordersToSeed = new List<Order>();

        if (customers.Any() && products.Any())
        {
            for (int i = 0; i < 100; i++)
            {
                var customer = faker.PickRandom(customers);
                var orderDate = faker.Date.Past(1); // Within last year
                var status = faker.PickRandom<OrderStatus>();
                var paymentMethod = faker.PickRandom(new[] { "Tiền mặt (COD)", "Chuyển khoản ngân hàng", "Ví MoMo", "Thẻ tín dụng" });

                var order = new Order
                {
                    CustomerId = customer.Id,
                    OrderDate = orderDate,
                    Status = status,
                    Address = faker.Address.FullAddress(),
                    Phone = customer.Phone ?? faker.Phone.PhoneNumber("09########"),
                    PaymentMethod = paymentMethod,
                    Items = new List<OrderItem>()
                };

                // Add 1-5 random items
                int itemCount = faker.Random.Int(1, 5);
                decimal total = 0;
                var selectedProducts = faker.PickRandom(products, itemCount);

                foreach (var p in selectedProducts)
                {
                    int qty = faker.Random.Int(1, 3);
                    var item = new OrderItem
                    {
                        ProductId = p.Id,
                        Quantity = qty,
                        Price = p.Price // Use current price
                    };
                    order.Items.Add(item);
                    total += item.Price * item.Quantity;
                }

                order.TotalAmount = total;
                ordersToSeed.Add(order);
            }

            context.Orders.AddRange(ordersToSeed);
            context.SaveChanges();
        }
    }
}

// Middleware Configuration
app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Assets and Static Files
app.MapStaticAssets();

// Route Mapping - Specific Routes First
app.MapAreaControllerRoute(
    name: "admin_dashboard",
    areaName: "Admin",
    pattern: "Admin",
    defaults: new { controller = "Admin", action = "Index" });

app.MapAreaControllerRoute(
    name: "customer_login",
    areaName: "Customer",
    pattern: "Account/Login",
    defaults: new { controller = "Account", action = "Login" });

// Area Routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Default Route (Defaults to Customer Area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Customer" });

// Run the application
app.Run();
