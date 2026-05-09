using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Data;
using PharmaSphere.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaSphere.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class ProductsController : Controller
    {
        private readonly PharmaContext _context;

        public ProductsController(PharmaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the list of products with search, filtering and pagination.
        /// </summary>
        public async Task<IActionResult> Index(string searchTerm, int? categoryId, int page = 1)
        {
            int pageSize = 5;
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var totalItems = await query.CountAsync();
            var products = await query.OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;

            return View(products);
        }

        /// <summary>
        /// Displays the form to create a new product.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
            return View(new Product());
        }

        /// <summary>
        /// Processes the creation of a new product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm sản phẩm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
            return View(product);
        }

        /// <summary>
        /// Displays the form to edit an existing product.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
            return View(product);
        }

        /// <summary>
        /// Processes the update of an existing product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Cập nhật sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Brands = await _context.Brands.ToListAsync();
            return View(product);
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa sản phẩm thành công.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
