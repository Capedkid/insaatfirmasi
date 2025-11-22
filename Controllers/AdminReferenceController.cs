using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

[Authorize]
public class AdminReferenceController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminReferenceController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminReferenceController(
        ApplicationDbContext context,
        ILogger<AdminReferenceController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var logos = await _context.ReferenceLogos
            .OrderBy(r => r.SortOrder)
            .ThenByDescending(r => r.CreatedDate)
            .ToListAsync();

        return View(logos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Lütfen bir referans logo görseli seçin.");

            var logosForError = await _context.ReferenceLogos
                .OrderBy(r => r.SortOrder)
                .ThenByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View("Index", logosForError);
        }

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "references");
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

            var logo = new ReferenceLogo
            {
                ImagePath = Path.Combine("uploads", "references", uniqueFileName).Replace("\\", "/"),
                CreatedDate = DateTime.Now
            };

            _context.ReferenceLogos.Add(logo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Referans logosu yüklenirken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Logo kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.");

            var logosForError = await _context.ReferenceLogos
                .OrderBy(r => r.SortOrder)
                .ThenByDescending(r => r.CreatedDate)
                .ToListAsync();

            return View("Index", logosForError);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var logo = await _context.ReferenceLogos.FindAsync(id);
        if (logo != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(logo.ImagePath))
                {
                    var physicalPath = Path.Combine(
                        _env.WebRootPath,
                        logo.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Referans logosu silinirken dosya silinemedi.");
            }

            _context.ReferenceLogos.Remove(logo);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}


