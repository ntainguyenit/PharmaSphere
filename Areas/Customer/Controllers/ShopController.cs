using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaSphere.Areas.Customer.Controllers
{
    /// <summary>
    /// Controller for browsing and viewing products in the Shop.
    /// </summary>
    [Area("Customer")]
    public class ShopController : Controller
    {
        private readonly PharmaContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShopController"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ShopController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the shop index page with filtering and search options.
        /// </summary>
        /// <param name="cat">Optional category ID filter.</param>
        /// <param name="minPrice">Optional minimum price filter.</param>
        /// <param name="maxPrice">Optional maximum price filter.</param>
        /// <param name="q">Optional search query.</param>
        /// <returns>The shop view with filtered products.</returns>
        public async Task<IActionResult> Index(int? cat, decimal? minPrice, decimal? maxPrice, string q, string type, int page = 1)
        {
            int pageSize = 4;
            var query = _context.Products.AsQueryable();

            if (cat.HasValue && cat.Value > 0)
                query = query.Where(p => p.CategoryId == cat.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q) || p.Description.Contains(q));

            if (type == "prescription")
                query = query.Where(p => p.IsPrescription);
            else if (type == "non-prescription")
                query = query.Where(p => p.IsPrescription == false);

            int totalItems = await query.CountAsync();
            int totalPages = (int)System.Math.Ceiling(totalItems / (double)pageSize);
            
            // Ensure page is within range
            if (page < 1) page = 1;
            if (totalPages > 0 && page > totalPages) page = totalPages;

            var products = await query
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.Take(5).ToListAsync();
            
            // Pagination Metadata
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;
            ViewBag.SelectedCategory = cat;
            ViewBag.SelectedType = type;
            ViewBag.SearchQuery = q;
            
            return View(products);
        }

        /// <summary>
        /// Displays details for a specific product.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>The product details view or NotFound if the product doesn't exist.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Get related products from same category
            ViewBag.RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            return View(product);
        }
    }
}
