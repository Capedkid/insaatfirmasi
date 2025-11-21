using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ILogger<CategoryController> logger)
    {
        _logger = logger;
    }

    // Tüm kategorileri listele (şimdilik boş liste)
    public IActionResult Index()
    {
        return View(new List<Category>());
    }

    // Kategori detayı ve ürünleri (şimdilik bulunamadı)
    public IActionResult Details(int? id)
    {
        return NotFound();
    }

    // Kategoriye göre ürünleri listele (şimdilik boş)
    public IActionResult Products(int? id)
    {
        ViewBag.Category = null;
        return View(new List<Product>());
    }

    // Kategori menüsü için partial view (şimdilik boş liste)
    public IActionResult Menu()
    {
        return PartialView("_CategoryMenu", new List<Category>());
    }
}
