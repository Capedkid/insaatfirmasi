using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class CatalogController : Controller
{
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    // Tüm katalogları listele (şimdilik boş liste)
    public IActionResult Index()
    {
        return View(new List<Catalog>());
    }

    // Katalog detayı (şimdilik bulunamadı)
    public IActionResult Details(int? id)
    {
        return NotFound();
    }

    // Katalog indirme (şimdilik bulunamadı)
    public IActionResult Download(int? id)
    {
        return NotFound();
    }

    // Katalog görüntüleme (şimdilik bulunamadı)
    public IActionResult View(int? id)
    {
        return NotFound();
    }
}
