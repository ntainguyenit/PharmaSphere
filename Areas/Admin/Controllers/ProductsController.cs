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
        private readonly IProductService _productService;
        private readonly IAuditService _auditService;

        public ProductsController(IProductService productService, IAuditService auditService)
        {
            _productService = productService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index(string searchTerm, int? categoryId, int page = 1)
        {
            int pageSize = 5;
            var products = await _productService.GetProductsAsync(searchTerm, categoryId, page, pageSize);
            var totalItems = await _productService.GetTotalProductCountAsync(searchTerm, categoryId);

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                bool success = await _productService.CreateProductAsync(product);
                if (success)
                {
                    await _auditService.LogAsync("Product", product.Id.ToString(), "Create", $"Sản phẩm {product.Name} đã được tạo.", User.Identity?.Name ?? "Admin");
                    TempData["Success"] = "Thêm sản phẩm mới thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                bool success = await _productService.UpdateProductAsync(product);
                if (success)
                {
                    await _auditService.LogAsync("Product", product.Id.ToString(), "Update", $"Sản phẩm {product.Name} đã được cập nhật.", User.Identity?.Name ?? "Admin");
                    TempData["Success"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.Brands = await _productService.GetBrandsAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product != null)
            {
                bool success = await _productService.DeleteProductAsync(id);
                if (success)
                {
                    await _auditService.LogAsync("Product", id.ToString(), "Delete", $"Sản phẩm {product.Name} đã bị xóa.", User.Identity?.Name ?? "Admin");
                    TempData["Success"] = "Đã xóa sản phẩm thành công.";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
