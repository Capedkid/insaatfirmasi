using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class BlogController : Controller
{
    private readonly ILogger<BlogController> _logger;

    public BlogController(ILogger<BlogController> logger)
    {
        _logger = logger;
    }

    // Tüm blog yazılarını listele (şimdilik boş liste)
    public IActionResult Index(int page = 1, int pageSize = 6)
    {
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = 1;
        ViewBag.TotalCount = 0;

        return View(new List<BlogPost>());
    }

    // Blog yazısı detayı (şimdilik bulunamadı)
    public IActionResult Details(int? id)
    {
        return NotFound();
    }

    // Kategoriye göre blog yazıları (şimdilik Index'e yönlendir)
    public IActionResult Category(string? category)
    {
        return RedirectToAction(nameof(Index));
    }

    // Blog yazısı arama (şimdilik Index'e yönlendir)
    [HttpPost]
    public IActionResult Search(string searchTerm)
    {
        return RedirectToAction(nameof(Index));
    }

    // Öne çıkan blog yazıları (şimdilik boş liste)
    public IActionResult Featured()
    {
        return View(new List<BlogPost>());
    }

    // RSS Feed (şimdilik boş liste)
    public IActionResult Rss()
    {
        return View(new List<BlogPost>());
    }
}
