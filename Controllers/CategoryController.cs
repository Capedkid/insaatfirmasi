using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Models;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Tüm kategorileri listele
    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        return View(categories);
    }

    // Kategori detayı ve ürünleri
    public async Task<IActionResult> Details(int? id)
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

        var otherCategories = await _context.Categories
            .Where(c => c.Id != id && c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        ViewBag.Products = products;
        ViewBag.OtherCategories = otherCategories;

        return View(category);
    }

    // Kategoriye göre ürünleri listele
    public async Task<IActionResult> Products(int? id)
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

    // Kategori menüsü için partial view
    public async Task<IActionResult> Menu()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        return PartialView("_CategoryMenu", categories);
    }
}
