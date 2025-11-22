using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class AdminContactController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminContactController> _logger;

    public AdminContactController(
        ApplicationDbContext context,
        ILogger<AdminContactController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
        
        if (contactInfo == null)
        {
            contactInfo = new ContactInfo { Id = 1 };
            _context.ContactInfos.Add(contactInfo);
            await _context.SaveChangesAsync();
        }

        return View(contactInfo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(ContactInfo contactInfo)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", contactInfo);
        }

        try
        {
            var existing = await _context.ContactInfos.FirstOrDefaultAsync();
            
            if (existing == null)
            {
                contactInfo.Id = 1;
                contactInfo.UpdatedDate = DateTime.Now;
                _context.ContactInfos.Add(contactInfo);
            }
            else
            {
                existing.Phone = contactInfo.Phone;
                existing.Email = contactInfo.Email;
                existing.Address = contactInfo.Address;
                existing.Latitude = contactInfo.Latitude;
                existing.Longitude = contactInfo.Longitude;
                existing.WhatsApp = contactInfo.WhatsApp;
                existing.UpdatedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "İletişim bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "İletişim bilgileri güncellenirken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Bilgiler güncellenirken bir hata oluştu. Lütfen tekrar deneyin.");
            return View("Index", contactInfo);
        }
    }
}

