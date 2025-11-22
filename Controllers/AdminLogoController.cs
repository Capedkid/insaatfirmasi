using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

[Authorize]
public class AdminLogoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminLogoController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminLogoController(
        ApplicationDbContext context,
        ILogger<AdminLogoController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var logos = await _context.SiteLogos
            .OrderByDescending(l => l.CreatedDate)
            .ToListAsync();

        return View(logos);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormFile? imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Lütfen bir logo görseli seçin.");

            var logosForError = await _context.SiteLogos
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();

            return View("Index", logosForError);
        }

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "logos");
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

            var logo = new SiteLogo
            {
                ImagePath = Path.Combine("uploads", "logos", uniqueFileName).Replace("\\", "/"),
                CreatedDate = DateTime.Now
            };

            _context.SiteLogos.Add(logo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logo yüklenirken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Logo kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.");

            var logosForError = await _context.SiteLogos
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();

            return View("Index", logosForError);
        }
    }
}


