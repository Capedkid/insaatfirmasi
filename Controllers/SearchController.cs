using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.Controllers;

public class SearchController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SearchController> _logger;

    public SearchController(ApplicationDbContext context, ILogger<SearchController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Global(string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return Json(Array.Empty<object>());
        }

        term = term.Trim();

        try
        {
            // Ürün araması
            var productResults = await _context.Products
                .Where(p => p.IsActive && p.Name != null && p.Name.Contains(term))
                .OrderBy(p => p.Name)
                .Take(5)
                .Select(p => new
                {
                    type = "product",
                    name = p.Name,
                    category = p.Category != null ? p.Category.Name : null,
                    url = Url.Action("Details", "Product", new { id = p.Id })
                })
                .ToListAsync();

            // Kategori araması
            var categoryResults = await _context.Categories
                .Where(c => c.IsActive && c.Name.Contains(term))
                .OrderBy(c => c.Name)
                .Take(5)
                .Select(c => new
                {
                    type = "category",
                    name = c.Name,
                    category = (string?)null,
                    url = Url.Action("Category", "Product", new { id = c.Id })
                })
                .ToListAsync();

            var results = productResults.Concat(categoryResults).ToList();

            return Json(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Global arama yapılırken bir hata oluştu.");
            return Json(Array.Empty<object>());
        }
    }
}


