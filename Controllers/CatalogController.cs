using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Models;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.Controllers;

public class CatalogController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ApplicationDbContext context, ILogger<CatalogController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Tüm katalogları listele
    public async Task<IActionResult> Index()
    {
        var catalogs = await _context.Catalogs
            .Where(c => c.IsActive)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        return View(catalogs);
    }

    // Katalog detayı
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalog = await _context.Catalogs
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (catalog == null)
        {
            return NotFound();
        }

        return View(catalog);
    }

    // Katalog indirme
    public async Task<IActionResult> Download(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalog = await _context.Catalogs
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (catalog == null)
        {
            return NotFound();
        }

        // İndirme sayısını artır
        catalog.DownloadCount++;
        await _context.SaveChangesAsync();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", catalog.FilePath);

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("Dosya bulunamadı.");
        }

        var contentType = catalog.FileExtension?.ToLower() switch
        {
            "pdf" => "application/pdf",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _ => "application/octet-stream"
        };

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        var fileName = $"{catalog.Name}.{catalog.FileExtension}";

        return File(fileBytes, contentType, fileName);
    }

    // Katalog görüntüleme (PDF için)
    public async Task<IActionResult> View(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var catalog = await _context.Catalogs
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

        if (catalog == null)
        {
            return NotFound();
        }

        if (catalog.FileExtension?.ToLower() != "pdf")
        {
            return RedirectToAction(nameof(Download), new { id });
        }

        return View(catalog);
    }
}
