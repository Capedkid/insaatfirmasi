using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

[Authorize]
public class AdminCorporateController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminCorporateController> _logger;

    public AdminCorporateController(
        ApplicationDbContext context,
        ILogger<AdminCorporateController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var content = await _context.CorporatePageContents.FirstOrDefaultAsync()
                      ?? new CorporatePageContent { Id = 1 };

        return View(content);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(CorporatePageContent content)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", content);
        }

        try
        {
            var existing = await _context.CorporatePageContents.FirstOrDefaultAsync();

            if (existing == null)
            {
                content.Id = 1;
                content.UpdatedDate = DateTime.Now;
                _context.CorporatePageContents.Add(content);
            }
            else
            {
                existing.HistoryTitle = content.HistoryTitle;
                existing.HistoryText1 = content.HistoryText1;
                existing.HistoryText2 = content.HistoryText2;

                existing.HistoryItem1Title = content.HistoryItem1Title;
                existing.HistoryItem1Text = content.HistoryItem1Text;
                existing.HistoryItem2Title = content.HistoryItem2Title;
                existing.HistoryItem2Text = content.HistoryItem2Text;
                existing.HistoryItem3Title = content.HistoryItem3Title;
                existing.HistoryItem3Text = content.HistoryItem3Text;
                existing.HistoryItem4Title = content.HistoryItem4Title;
                existing.HistoryItem4Text = content.HistoryItem4Text;

                existing.MissionTitle = content.MissionTitle;
                existing.MissionText = content.MissionText;
                existing.VisionTitle = content.VisionTitle;
                existing.VisionText = content.VisionText;

                existing.ValuesTitle = content.ValuesTitle;
                existing.ValuesSubtitle = content.ValuesSubtitle;
                existing.Value1Title = content.Value1Title;
                existing.Value1Text = content.Value1Text;
                existing.Value2Title = content.Value2Title;
                existing.Value2Text = content.Value2Text;
                existing.Value3Title = content.Value3Title;
                existing.Value3Text = content.Value3Text;
                existing.Value4Title = content.Value4Title;
                existing.Value4Text = content.Value4Text;

                existing.UpdatedDate = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kurumsal sayfası metni başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kurumsal sayfası güncellenirken bir hata oluştu.");
            ModelState.AddModelError(string.Empty, "Metin kaydedilirken bir hata oluştu. Lütfen tekrar deneyin.");
            return View("Index", content);
        }
    }
}


