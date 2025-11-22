using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

[Authorize]
public class AdminProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminProductController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminProductController(
        ApplicationDbContext context,
        ILogger<AdminProductController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .ToListAsync();

        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        ViewBag.Categories = categories;

        return View(products);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .ToListAsync();

        ViewBag.Categories = categories;

        if (!categories.Any(c => c.Id == product.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Lütfen bir kategori seçin.");
        }

        if (!ModelState.IsValid)
        {
            var productsForInvalid = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.SortOrder)
                .ThenBy(p => p.Name)
                .ToListAsync();

            return View("Index", productsForInvalid);
        }

        if (imageFile != null && imageFile.Length > 0)
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            product.MainImagePath = Path.Combine("uploads", "products", uniqueFileName).Replace("\\", "/");
        }

        // Varsayılanlar
        product.IsActive = true;

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(product.MainImagePath))
                {
                    var physicalPath = Path.Combine(
                        _env.WebRootPath,
                        product.MainImagePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ürün silinirken resim dosyası silinemedi.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}


