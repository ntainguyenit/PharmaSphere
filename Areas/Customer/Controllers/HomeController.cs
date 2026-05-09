using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Models;
using PharmaSphere.Data;

namespace PharmaSphere.Areas.Customer.Controllers
{
    /// <summary>
    /// Controller handling the main landing pages for the Customer area.
    /// </summary>
    [Area("Customer")]
public class HomeController : Controller
{
    private readonly PharmaContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="HomeController"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public HomeController(PharmaContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays the home page with categories and featured products.
    /// </summary>
    /// <returns>The home view.</returns>
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        var featuredProducts = await _context.Products
            .Include(p => p.Brand)
            .OrderBy(p => p.Id)
            .Take(6)
            .ToListAsync();
        
        ViewBag.Categories = categories;
        ViewBag.FeaturedProducts = featuredProducts;
        
        return View();
    }

    /// <summary>
    /// Displays the privacy policy page.
    /// </summary>
    /// <returns>The privacy view.</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays the promotions page.
    /// </summary>
    /// <returns>The promotions view.</returns>
    public IActionResult Promotions()
    {
        return View();
    }

    /// <summary>
    /// Displays the news page.
    /// </summary>
    /// <returns>The news view.</returns>
    public IActionResult News()
    {
        return View();
    }

    /// <summary>
    /// Displays the error page.
    /// </summary>
    /// <returns>The error view with request ID.</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
}
