using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Models;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Öne çıkan ürünler
        var featuredProducts = await _context.Products
            .Where(p => p.IsActive && p.IsFeatured)
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .Take(6)
            .ToListAsync();

        // Aktif kategoriler
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        // Son blog yazıları
        var recentBlogPosts = await _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue)
            .OrderByDescending(b => b.PublishedDate)
            .Take(3)
            .ToListAsync();

        ViewBag.FeaturedProducts = featuredProducts;
        ViewBag.Categories = categories;
        ViewBag.RecentBlogPosts = recentBlogPosts;

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
