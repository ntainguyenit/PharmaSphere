using Microsoft.AspNetCore.Mvc;
using PharmaSphere.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace PharmaSphere.ViewComponents
{
    /// <summary>
    /// ViewComponent to display a summary of inventory alerts.
    /// </summary>
    public class InventoryAlertsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public InventoryAlertsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productService.GetProductsAsync(null, null, 1, 100);
            var lowStockItems = products.Where(p => p.Stock <= 10).ToList();
            
            return View(lowStockItems);
        }
    }

    /// <summary>
    /// ViewComponent to display the mini cart in the header.
    /// </summary>
    public class CartSummaryViewComponent : ViewComponent
    {
        // Logic to get cart from session/database
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int itemCount = 3; // Mock
            decimal total = 450000; // Mock
            
            ViewBag.ItemCount = itemCount;
            ViewBag.Total = total;
            
            return await Task.FromResult(View());
        }
    }

    /// <summary>
    /// ViewComponent for the category sidebar.
    /// </summary>
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public CategoryMenuViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _productService.GetCategoriesAsync();
            return View(categories);
        }
    }
}
