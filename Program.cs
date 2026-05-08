using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PharmaSphere.Data;

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
    var names = new[] { "Thuốc cảm cúm", "Vitamin", "Tim mạch", "Tiểu đường", "Thiết bị y tế", "Chăm sóc cá nhân", "Dược mỹ phẩm", "Mẹ và Bé", "Thực phẩm chức năng", "Hỗ trợ tiêu hóa" };
    var cats = context.Categories.OrderBy(c => c.Id).ToList();
    for (int i = 0; i < Math.Min(cats.Count, names.Length); i++) 
    {
        cats[i].Name = names[i];
    }
    context.SaveChanges();
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
