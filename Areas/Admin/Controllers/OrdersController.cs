using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace PharmaSphere.Areas.Admin.Controllers
{
    /// <summary>
    /// Controller for managing customer orders in the Admin area.
    /// </summary>
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class OrdersController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public OrdersController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a paginated list of orders with search and status filtering.
        /// </summary>
        /// <param name="searchTerm">The term to search for in customer name or phone.</param>
        /// <param name="status">The order status to filter by.</param>
        /// <param name="page">The current page number.</param>
        /// <returns>A view with the paginated list of orders.</returns>
        public async Task<IActionResult> Index(string searchTerm, OrderStatus? status, int page = 1)
        {
            int pageSize = 5;
            var query = _context.Orders
                .Include(o => o.Customer)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(o => o.Customer.FullName.Contains(searchTerm) || o.Phone.Contains(searchTerm) || o.Id.ToString().Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            var totalItems = await query.CountAsync();
            var orders = await query.OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StatusFilter = status;

            return View(orders);
        }

        /// <summary>
        /// Exports the orders list to an Excel file.
        /// </summary>
        public async Task<IActionResult> ExportExcel()
        {
            var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Orders");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Mã ĐH";
                worksheet.Cell(currentRow, 2).Value = "Khách hàng";
                worksheet.Cell(currentRow, 3).Value = "Ngày đặt";
                worksheet.Cell(currentRow, 4).Value = "Tổng tiền";
                worksheet.Cell(currentRow, 5).Value = "Trạng thái";

                foreach (var order in orders)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = "#" + order.Id.ToString("D5");
                    worksheet.Cell(currentRow, 2).Value = order.Customer.FullName;
                    worksheet.Cell(currentRow, 3).Value = order.OrderDate.ToString("dd/MM/yyyy HH:mm");
                    worksheet.Cell(currentRow, 4).Value = order.TotalAmount;
                    worksheet.Cell(currentRow, 5).Value = order.Status.ToString();
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Orders_{DateTime.Now:yyyyMMdd}.xlsx");
                }
            }
        }

        /// <summary>
        /// Exports the orders list to a PDF file using QuestPDF.
        /// </summary>
        public async Task<IActionResult> ExportPdf()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var orders = await _context.Orders.Include(o => o.Customer).ToListAsync();
            
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.Header().Text("PHARMASPHERE - DANH SÁCH ĐƠN HÀNG").FontSize(20).SemiBold().FontColor(Colors.Green.Darken2);
                    
                    page.Content().PaddingVertical(1, Unit.Centimetre).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Mã ĐH");
                            header.Cell().Element(CellStyle).Text("Khách hàng");
                            header.Cell().Element(CellStyle).Text("Ngày đặt");
                            header.Cell().Element(CellStyle).Text("Tổng tiền");
                            header.Cell().Element(CellStyle).Text("Trạng thái");

                            static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        });

                        foreach (var order in orders)
                        {
                            table.Cell().Element(CellStyle).Text("#" + order.Id.ToString("D5"));
                            table.Cell().Element(CellStyle).Text(order.Customer.FullName);
                            table.Cell().Element(CellStyle).Text(order.OrderDate.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Element(CellStyle).Text(order.TotalAmount.ToString("N0") + " ₫");
                            table.Cell().Element(CellStyle).Text(order.Status.ToString());

                            static IContainer CellStyle(IContainer container) => container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                        }
                    });
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return File(stream.ToArray(), "application/pdf", $"Orders_{DateTime.Now:yyyyMMdd}.pdf");
            }
        }

        /// <summary>
        /// Displays the details of a specific order.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>A view with the order details or NotFound.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <param name="status">The new status.</param>
        /// <returns>A redirect to the Index view.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Confirms the deletion of an order.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>A redirect to the Index view.</returns>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
