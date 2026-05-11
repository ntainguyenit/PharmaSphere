using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PharmaSphere.Models;
using PharmaSphere.Services.Interfaces;

namespace PharmaSphere.Services.Implementations
{
    public class DataExportService : IDataExportService
    {
        public async Task<string> ExportProductsToCsvAsync(IEnumerable<Product> products)
        {
            var sb = new StringBuilder();
            sb.AppendLine("ID,Name,Price,Stock,Category,Brand,Status");

            foreach (var p in products)
            {
                sb.AppendLine($"{p.Id},{EscapeCsv(p.Name)},{p.Price},{p.Stock},{EscapeCsv(p.Category?.Name)},{EscapeCsv(p.Brand?.Name)},{p.Status}");
            }

            return await Task.FromResult(sb.ToString());
        }

        public async Task<string> ExportOrdersToCsvAsync(IEnumerable<Order> orders)
        {
            var sb = new StringBuilder();
            sb.AppendLine("OrderID,Date,Customer,Total,Status,Payment");

            foreach (var o in orders)
            {
                sb.AppendLine($"{o.Id},{o.OrderDate:yyyy-MM-dd HH:mm},{EscapeCsv(o.Customer?.FullName)},{o.TotalAmount},{o.Status},{EscapeCsv(o.PaymentMethod)}");
            }

            return await Task.FromResult(sb.ToString());
        }

        public async Task<byte[]> GenerateInventoryReportPdfAsync()
        {
            // In a real app, use a library like QuestPDF or iText7
            // Here we just return mock bytes
            return await Task.FromResult(new byte[1024]);
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
}
