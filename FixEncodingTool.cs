using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace PharmaSphere
{
    /// <summary>
    /// Utility tool to fix encoding issues in the database by re-seeding category names with proper Vietnamese characters.
    /// </summary>
    public class Fixer
    {
        /// <summary>
        /// Executes the fix logic.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Execute(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            
            var optionsBuilder = new DbContextOptionsBuilder<PharmaContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            using (var context = new PharmaContext(optionsBuilder.Options))
            {
                Console.WriteLine("Fixing encoding...");
                
                var categories = context.Categories.ToList();
                var categoryNames = new[] {
                    "Thuốc cảm cúm", "Vitamin", "Tim mạch", "Tiểu đường", 
                    "Thiết bị y tế", "Chăm sóc cá nhân", "Dược mỹ phẩm", 
                    "Mẹ và Bé", "Thực phẩm chức năng", "Hỗ trợ tiêu hóa"
                };

                for (int i = 0; i < Math.Min(categories.Count, categoryNames.Length); i++)
                {
                    categories[i].Name = categoryNames[i];
                }

                context.SaveChanges();
                Console.WriteLine("Encoding fixed successfully!");
            }
        }
    }
}
