using Microsoft.AspNetCore.Mvc;
using PharmaSphere.Data;
using PharmaSphere.Models;
using PharmaSphere.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace PharmaSphere.Areas.Customer.Controllers
{
    /// <summary>
    /// Controller for managing the shopping cart and checkout process.
    /// </summary>
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly PharmaContext _context;
        private const string CartSessionKey = "Cart";

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CartController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the shopping cart index page.
        /// </summary>
        /// <returns>The cart view with current items.</returns>
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            return View(cart);
        }

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="id">The product ID to add.</param>
        /// <param name="quantity">The quantity to add (default is 1).</param>
        /// <returns>A redirect to the cart index page or NotFound if the product doesn't exist.</returns>
        [HttpPost]
        public IActionResult Add(int id, int quantity = 1)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();

            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.Set(CartSessionKey, cart);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Updates the quantity of a product in the cart.
        /// </summary>
        /// <param name="id">The product ID to update.</param>
        /// <param name="quantity">The new quantity.</param>
        /// <returns>A redirect to the cart index page.</returns>
        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            
            if (item != null)
            {
                if (quantity > 0)
                {
                    item.Quantity = quantity;
                }
                else
                {
                    cart.Remove(item);
                }
                HttpContext.Session.Set(CartSessionKey, cart);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Removes a product from the cart.
        /// </summary>
        /// <param name="id">The product ID to remove.</param>
        /// <returns>A redirect to the cart index page.</returns>
        [HttpPost]
        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            var item = cart.FirstOrDefault(c => c.ProductId == id);
            
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(CartSessionKey, cart);
            }

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Displays the checkout page.
        /// </summary>
        /// <returns>The checkout view or a redirect to the shop if the cart is empty.</returns>
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Shop");
            }
            
            return View(cart);
        }
        
        /// <summary>
        /// Processes the order placement.
        /// </summary>
        /// <param name="address">The shipping address.</param>
        /// <param name="phone">The contact phone number.</param>
        /// <param name="paymentMethod">The chosen payment method.</param>
        /// <returns>A redirect to the success page or back to shop/login if necessary.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(string address, string phone, string paymentMethod)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>(CartSessionKey) ?? new List<CartItemViewModel>();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Shop");
            }

            // Requires user to be logged in to place order
            if (!User.Identity.IsAuthenticated || !User.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier))
            {
                return RedirectToAction("Login", "Account", new { area = "Customer", returnUrl = "/Cart/Checkout" });
            }

            int customerId = int.Parse(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

            var order = new Order
            {
                OrderDate = System.DateTime.Now,
                CustomerId = customerId,
                Address = address,
                Phone = phone,
                PaymentMethod = paymentMethod,
                Status = OrderStatus.Pending,
                TotalAmount = cart.Sum(c => c.Total),
                Items = cart.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Clear Cart
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("Success", new { id = order.Id });
        }
        
        /// <summary>
        /// Displays the order success page.
        /// </summary>
        /// <param name="id">The completed order ID.</param>
        /// <returns>The success view.</returns>
        public IActionResult Success(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
