using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    // Tüm ürünleri listele (şimdilik boş liste ile tasarım gösterimi)
    public IActionResult Index(int? categoryId, string? search)
    {
        ViewBag.Categories = new List<Category>();
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = search;

        return View(new List<Product>());
    }

    // Ürün detayı (şimdilik bulunamadı döner)
    public IActionResult Details(int? id)
    {
        return NotFound();
    }

    // Kategoriye göre ürünleri listele (şimdilik boş)
    public IActionResult Category(int? id)
    {
        ViewBag.Category = null;
        return View(new List<Product>());
    }

    // Arama sonuçları (şimdilik Index'e yönlendir)
    [HttpPost]
    public IActionResult Search(string searchTerm)
    {
        return RedirectToAction(nameof(Index), new { search = searchTerm });
    }
}
