using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PharmaSphere.Models;

namespace PharmaSphere.Services.Interfaces
{
    public interface IDataExportService
    {
        Task<string> ExportProductsToCsvAsync(IEnumerable<Product> products);
        Task<string> ExportOrdersToCsvAsync(IEnumerable<Order> orders);
        Task<byte[]> GenerateInventoryReportPdfAsync();
    }
}
