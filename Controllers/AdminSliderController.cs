using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class AdminSliderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminSliderController> _logger;
    private readonly IWebHostEnvironment _env;

    public AdminSliderController(
        ApplicationDbContext context,
        ILogger<AdminSliderController> logger,
        IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync();

        return View(sliders);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Slider slider, IFormFile? imageFile)
    {
        if (!ModelState.IsValid)
        {
            var slidersInvalid = await _context.Sliders
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View("Index", slidersInvalid);
        }

        try
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "sliders");
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

                slider.ImagePath = Path.Combine("uploads", "sliders", uniqueFileName).Replace("\\", "/");
            }

            slider.IsActive = true;
            slider.CreatedDate = DateTime.Now;
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Slider oluşturulurken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Slider kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.");

            var slidersError = await _context.Sliders
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return View("Index", slidersError);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.Sliders.FindAsync(id);
        if (slider != null)
        {
            try
            {
                if (!string.IsNullOrEmpty(slider.ImagePath))
                {
                    var physicalPath = Path.Combine(
                        _env.WebRootPath,
                        slider.ImagePath.Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Slider silinirken görsel dosyası silinemedi.");
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}


