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

        // Slider kayıtları
        var sliders = await _context.Sliders
            .Where(s => s.IsActive)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync();

        // Hakkımızda görselleri
        var aboutImages = await _context.AboutImages
            .OrderBy(a => a.SortOrder)
            .ThenByDescending(a => a.CreatedDate)
            .ToListAsync();

        // Referans logoları
        var referenceLogos = await _context.ReferenceLogos
            .OrderBy(r => r.SortOrder)
            .ThenByDescending(r => r.CreatedDate)
            .ToListAsync();

        // Bölüm metinleri
        var aboutContent = await _context.AboutSectionContents.FirstOrDefaultAsync();
        var referenceContent = await _context.ReferenceSectionContents.FirstOrDefaultAsync();

        ViewBag.FeaturedProducts = featuredProducts;
        ViewBag.Categories = categories;
        ViewBag.RecentBlogPosts = recentBlogPosts;
        ViewBag.Sliders = sliders;
        ViewBag.AboutImages = aboutImages;
        ViewBag.ReferenceLogos = referenceLogos;
        ViewBag.AboutContent = aboutContent;
        ViewBag.ReferenceContent = referenceContent;

        return View();
    }

    public async Task<IActionResult> About()
    {
        var content = await _context.CorporatePageContents.FirstOrDefaultAsync();
        return View(content);
    }

    public async Task<IActionResult> Contact()
    {
        var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
        ViewBag.ContactInfo = contactInfo;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
