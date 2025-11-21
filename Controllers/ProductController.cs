using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Models;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductController> _logger;

    public ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Tüm ürünleri listele
    public async Task<IActionResult> Index(int? categoryId, string? search)
    {
        var query = _context.Products
            .Where(p => p.IsActive)
            .AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p =>
                (p.Name != null && p.Name.Contains(search)) ||
                (p.Description != null && p.Description.Contains(search)) ||
                (p.ModelNumber != null && p.ModelNumber.Contains(search)));
        }

        var products = await query
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync();

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        ViewBag.Categories = categories;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SearchTerm = search;

        return View(products);
    }

    // Ürün detayı
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // Kategoriye göre ürünleri listele
    public async Task<IActionResult> Category(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (category == null)
        {
            return NotFound();
        }

        var products = await _context.Products
            .Where(p => p.CategoryId == id && p.IsActive)
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync();

        ViewBag.Category = category;

        return View(products);
    }

    // Arama sonuçları
    [HttpPost]
    public IActionResult Search(string searchTerm)
    {
        return RedirectToAction(nameof(Index), new { search = searchTerm });
    }
}
