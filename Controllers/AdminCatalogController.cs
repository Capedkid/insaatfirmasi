using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class AdminCatalogController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminCatalogController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminCatalogController(
        ApplicationDbContext context,
        ILogger<AdminCatalogController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var catalogs = await _context.Catalogs
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();

        return View(catalogs);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Catalog catalog, IFormFile? file, IFormFile? coverImage)
    {
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("FilePath", "Lütfen bir katalog dosyası (PDF vb.) yükleyin.");
        }

        if (!ModelState.IsValid)
        {
            var catalogsForInvalid = await _context.Catalogs
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View("Index", catalogsForInvalid);
        }

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "catalogs");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Ana katalog dosyası
            if (file != null && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var extensionWithoutDot = fileExtension.TrimStart('.').ToLowerInvariant();

                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                catalog.FilePath = Path.Combine("uploads", "catalogs", uniqueFileName).Replace("\\", "/");
                catalog.FileExtension = extensionWithoutDot;
                catalog.FileSizeBytes = file.Length;
                catalog.FileSize = FormatFileSize(file.Length);
            }

            // Kapak görseli (isteğe bağlı)
            if (coverImage != null && coverImage.Length > 0)
            {
                var coverFileName = $"{Guid.NewGuid()}{Path.GetExtension(coverImage.FileName)}";
                var coverPath = Path.Combine(uploadsFolder, coverFileName);

                using (var stream = new FileStream(coverPath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(stream);
                }

                catalog.CoverImagePath = Path.Combine("uploads", "catalogs", coverFileName).Replace("\\", "/");
            }

            catalog.IsActive = true;
            catalog.CreatedDate = DateTime.Now;
            catalog.DownloadCount = 0;

            _context.Catalogs.Add(catalog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Katalog oluşturulurken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Katalog kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.");

            var catalogsForError = await _context.Catalogs
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();

            return View("Index", catalogsForError);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var catalog = await _context.Catalogs.FindAsync(id);
        if (catalog != null)
        {
            try
            {
                // Ana dosya
                if (!string.IsNullOrEmpty(catalog.FilePath))
                {
                    var physicalFilePath = Path.Combine(
                        _env.WebRootPath,
                        catalog.FilePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalFilePath))
                    {
                        System.IO.File.Delete(physicalFilePath);
                    }
                }

                // Kapak görseli
                if (!string.IsNullOrEmpty(catalog.CoverImagePath))
                {
                    var physicalCoverPath = Path.Combine(
                        _env.WebRootPath,
                        catalog.CoverImagePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalCoverPath))
                    {
                        System.IO.File.Delete(physicalCoverPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Katalog silinirken dosyalar silinemedi.");
            }

            _context.Catalogs.Remove(catalog);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private static string FormatFileSize(long bytes)
    {
        const long KB = 1024;
        const long MB = KB * 1024;
        const long GB = MB * 1024;

        if (bytes >= GB)
        {
            return $"{bytes / (double)GB:0.##} GB";
        }
        if (bytes >= MB)
        {
            return $"{bytes / (double)MB:0.##} MB";
        }
        if (bytes >= KB)
        {
            return $"{bytes / (double)KB:0.##} KB";
        }

        return $"{bytes} B";
    }
}


