using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using PharmaSphere.Data;
using PharmaSphere.Models;
using System.Collections.Generic;
using PharmaSphere;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddPharmaServices();

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

// Custom Global Exception Middleware
app.UseMiddleware<PharmaSphere.Middlewares.GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Database initialization and seeding
using (var scope = app.Services.CreateScope())
{
    DbInitializer.Seed(scope.ServiceProvider);
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
