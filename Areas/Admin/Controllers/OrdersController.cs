using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;

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
        /// Displays a list of all orders.
        /// </summary>
        /// <returns>A view with the list of orders.</returns>
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
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
