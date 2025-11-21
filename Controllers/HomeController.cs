using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Şimdilik veritabanı kullanılmadığı için ViewBag'lere boş listeler veriyoruz.
        ViewBag.FeaturedProducts = new List<Product>();
        ViewBag.Categories = new List<Category>();
        ViewBag.RecentBlogPosts = new List<BlogPost>();

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
