using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PharmaSphere.Data;
using PharmaSphere.Models;
using BCrypt.Net;

namespace PharmaSphere
{
    /// <summary>
    /// Utility class to initialize and seed the database with initial data.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Seeds the database with system settings, users, categories, brands, products, and orders.
        /// </summary>
        /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
        public static void Seed(IServiceProvider serviceProvider)
        {
            using (var context = new PharmaContext(
                serviceProvider.GetRequiredService<DbContextOptions<PharmaContext>>()))
            {
                // Use Migrate instead of EnsureCreated to support migrations and schema updates
                context.Database.Migrate();

                // 1. Seed System Settings
                if (!context.SystemSettings.Any())
                {
                    context.SystemSettings.AddRange(
                        new SystemSetting { Key = "PharmacyName", Value = "PharmaSphere Premium", Description = "Tên nhà thuốc hiển thị trên hệ thống" },
                        new SystemSetting { Key = "ContactEmail", Value = "contact@pharmasphere.com", Description = "Email liên hệ chính thức" },
                        new SystemSetting { Key = "ContactPhone", Value = "1900 1234", Description = "Số điện thoại hotline" },
                        new SystemSetting { Key = "Address", Value = "123 Đường ABC, Quận 1, TP. Hồ Chí Minh", Description = "Địa chỉ trụ sở chính" },
                        new SystemSetting { Key = "LowStockThreshold", Value = "20", Description = "Ngưỡng cảnh báo tồn kho thấp" }
                    );
                }

                // 2. Seed Users (Admins, Staff, Customers)
                // Check if we need to reset/update users to have hashed passwords
                var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
                bool needsReseed = adminUser == null || !adminUser.PasswordHash.StartsWith("$2");

                if (needsReseed)
                {
                    // Clear existing users if they are not in the right format
                    // Note: We might need to clear related orders first to avoid FK errors
                    // For safety, let's just update the specific ones we need.
                    
                    void AddOrUpdateUser(string username, string password, string fullName, string email, string phone, UserRole role)
                    {
                        var user = context.Users.FirstOrDefault(u => u.Username == username);
                        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
                        
                        if (user == null)
                        {
                            context.Users.Add(new User {
                                Username = username,
                                PasswordHash = hashed,
                                FullName = fullName,
                                Email = email,
                                Phone = phone,
                                Role = role
                            });
                        }
                        else
                        {
                            user.PasswordHash = hashed;
                            user.FullName = fullName;
                            user.Email = email;
                            user.Phone = phone;
                            user.Role = role;
                        }
                    }

                    // 1 Admin
                    AddOrUpdateUser("admin", "admin", "Quản trị viên Hệ thống", "admin@pharmasphere.com", "0987654321", UserRole.Admin);
                    
                    // 1 Staff
                    AddOrUpdateUser("staff", "staff", "Nhân viên Vận hành", "staff@pharmasphere.com", "0991112223", UserRole.Staff);

                    // 10 Customers (user1 to user10)
                    for (int i = 1; i <= 10; i++)
                    {
                        AddOrUpdateUser($"user{i}", "123456", $"Khách hàng số {i}", $"user{i}@gmail.com", $"090000000{i % 10}", UserRole.Customer);
                    }
                    
                    context.SaveChanges();
                }

                // 3. Seed Categories
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(
                        new Category { Name = "Thuốc cảm cúm", Icon = "thermometer" },
                        new Category { Name = "Vitamin & Thực phẩm chức năng", Icon = "pill" },
                        new Category { Name = "Thiết bị y tế", Icon = "activity" },
                        new Category { Name = "Chăm sóc cá nhân", Icon = "sparkles" },
                        new Category { Name = "Dược mỹ phẩm", Icon = "flask-conical" }
                    );
                    context.SaveChanges();
                }

                // 4. Seed Brands
                if (!context.Brands.Any())
                {
                    context.Brands.AddRange(
                        new Brand { Name = "DHG Pharma" },
                        new Brand { Name = "Traphaco" },
                        new Brand { Name = "Sanofi" },
                        new Brand { Name = "GlaxoSmithKline" },
                        new Brand { Name = "Blackmores" }
                    );
                    context.SaveChanges();
                }

                // 5. Seed Products
                if (!context.Products.Any())
                {
                    var categories = context.Categories.ToList();
                    var brands = context.Brands.ToList();

                    if (categories.Any() && brands.Any())
                    {
                        context.Products.AddRange(
                            new Product { Name = "Hapacol 650", Description = "Thuốc giảm đau hạ sốt hiệu quả", Price = 45000, Stock = 150, CategoryId = categories[0].Id, BrandId = brands[0].Id, Rating = 4.8, ReviewCount = 120 },
                            new Product { Name = "Panadol Extra", Description = "Giảm đau đầu, đau răng, đau cơ", Price = 55000, Stock = 200, CategoryId = categories[0].Id, BrandId = brands[2].Id, Rating = 4.9, ReviewCount = 350 },
                            new Product { Name = "Vitamin C 500mg", Description = "Tăng cường sức đề kháng", Price = 85000, Stock = 80, CategoryId = categories[1].Id, BrandId = brands[0].Id, Rating = 4.5, ReviewCount = 85 },
                            new Product { Name = "Dầu cá Omega-3", Description = "Tốt cho tim mạch và trí não", Price = 250000, Stock = 45, CategoryId = categories[1].Id, BrandId = brands[2].Id, Rating = 4.7, ReviewCount = 210 },
                            new Product { Name = "Khẩu trang N95", Description = "Bảo vệ đường hô hỗ tối ưu", Price = 15000, Stock = 15, CategoryId = categories[2].Id, BrandId = brands[0].Id, Rating = 4.6, ReviewCount = 500 }
                        );
                        context.SaveChanges();
                    }
                }

                // 6. Seed Orders for Statistics
                if (!context.Orders.Any())
                {
                    var customers = context.Users.Where(u => u.Role == UserRole.Customer).ToList();
                    var products = context.Products.ToList();
                    var random = new Random();

                    if (customers.Any() && products.Any())
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            var orderDate = DateTime.Now.AddDays(-i);
                            var order = new Order
                            {
                                OrderDate = orderDate,
                                CustomerId = customers[random.Next(customers.Count)].Id,
                                TotalAmount = 0,
                                Status = i % 10 == 0 ? OrderStatus.Cancelled : OrderStatus.Delivered,
                                Address = "Địa chỉ khách hàng " + i,
                                Phone = "090000000" + (i % 10),
                                PaymentMethod = i % 2 == 0 ? "Tiền mặt" : "Chuyển khoản"
                            };

                            context.Orders.Add(order);
                            context.SaveChanges();

                            var itemCount = random.Next(1, 4);
                            decimal total = 0;
                            for (int j = 0; j < itemCount; j++)
                            {
                                var product = products[random.Next(products.Count)];
                                var quantity = random.Next(1, 5);
                                var orderItem = new OrderItem
                                {
                                    OrderId = order.Id,
                                    ProductId = product.Id,
                                    Quantity = quantity,
                                    Price = product.Price
                                };
                                context.OrderItems.Add(orderItem);
                                total += orderItem.Price * orderItem.Quantity;
                            }
                            order.TotalAmount = total;
                            context.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
