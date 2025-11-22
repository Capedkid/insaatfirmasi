using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;
using InsaatFirmasi.ViewModels;

namespace InsaatFirmasi.Controllers;

[Authorize]
public class AdminAboutController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminAboutController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminAboutController(
        ApplicationDbContext context,
        ILogger<AdminAboutController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var images = await _context.AboutImages
            .OrderBy(a => a.SortOrder)
            .ThenByDescending(a => a.CreatedDate)
            .ToListAsync();

        var content = await _context.AboutSectionContents.FirstOrDefaultAsync()
                      ?? new AboutSectionContent { Id = 1 };

        var vm = new AdminAboutViewModel
        {
            Images = images,
            Content = content
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormFile? imageFile, int? sortOrder)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Görsel dosyası seçilmelidir.");
            var imagesForError = await _context.AboutImages
                .OrderBy(a => a.SortOrder)
                .ThenByDescending(a => a.CreatedDate)
                .ToListAsync();
            var contentForError = await _context.AboutSectionContents.FirstOrDefaultAsync()
                                  ?? new AboutSectionContent { Id = 1 };
            var vmError = new AdminAboutViewModel
            {
                Images = imagesForError,
                Content = contentForError
            };
            return View("Index", vmError);
        }

        try
        {
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "about");
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

            var aboutImage = new AboutImage
            {
                ImagePath = Path.Combine("uploads", "about", uniqueFileName).Replace("\\", "/"),
                SortOrder = sortOrder ?? 0,
                CreatedDate = DateTime.Now
            };

            _context.AboutImages.Add(aboutImage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hakkımızda görseli yüklenirken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Görsel yüklenirken bir hata oluştu. Lütfen tekrar deneyin.");

            var imagesForError = await _context.AboutImages
                .OrderBy(a => a.SortOrder)
                .ThenByDescending(a => a.CreatedDate)
                .ToListAsync();
            var contentForError = await _context.AboutSectionContents.FirstOrDefaultAsync()
                                  ?? new AboutSectionContent { Id = 1 };
            var vmError = new AdminAboutViewModel
            {
                Images = imagesForError,
                Content = contentForError
            };

            return View("Index", vmError);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var image = await _context.AboutImages.FindAsync(id);
        if (image != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(image.ImagePath))
                {
                    var physicalPath = Path.Combine(
                        _env.WebRootPath,
                        image.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Hakkımızda görseli silinirken dosya silinemedi.");
            }

            _context.AboutImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateContent(AboutSectionContent content)
    {
        var existing = await _context.AboutSectionContents.FirstOrDefaultAsync();

        if (existing == null)
        {
            content.Id = 1;
            content.UpdatedDate = DateTime.Now;
            _context.AboutSectionContents.Add(content);
        }
        else
        {
            existing.Title = content.Title;
            existing.Subtitle = content.Subtitle;
            existing.Item1Title = content.Item1Title;
            existing.Item1Text = content.Item1Text;
            existing.Item2Title = content.Item2Title;
            existing.Item2Text = content.Item2Text;
            existing.Item3Title = content.Item3Title;
            existing.Item3Text = content.Item3Text;
            existing.UpdatedDate = DateTime.Now;
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Hakkımızda metni başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }
}

