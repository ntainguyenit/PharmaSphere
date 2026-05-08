using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaSphere.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShopController : Controller
    {
        private readonly PharmaContext _context;

        public ShopController(PharmaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? cat, decimal? minPrice, decimal? maxPrice, string q)
        {
            var query = _context.Products.AsQueryable();

            if (cat.HasValue)
                query = query.Where(p => p.CategoryId == cat.Value);

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            if (!string.IsNullOrEmpty(q))
                query = query.Where(p => p.Name.Contains(q) || p.Description.Contains(q));

            var products = await query.Include(p => p.Brand).Include(p => p.Category).ToListAsync();
            
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.Take(5).ToListAsync();
            
            return View(products);
        }

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
