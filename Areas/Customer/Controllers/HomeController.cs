using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSphere.Models;
using PharmaSphere.Data;

namespace PharmaSphere.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly PharmaContext _context;

    public HomeController(PharmaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();
        var featuredProducts = await _context.Products.Include(p => p.Brand).OrderBy(p => p.Id).Take(8).ToListAsync();
        
        ViewBag.Categories = categories;
        ViewBag.FeaturedProducts = featuredProducts;
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
