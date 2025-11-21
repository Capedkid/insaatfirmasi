using Microsoft.AspNetCore.Mvc;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class ContactController : Controller
{
    private readonly ILogger<ContactController> _logger;

    public ContactController(ILogger<ContactController> logger)
    {
        _logger = logger;
    }

    // İletişim sayfası
    public IActionResult Index()
    {
        return View();
    }

    // İletişim formu gönderme (şimdilik sadece loglayıp başarı mesajı gösteriyoruz)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactMessage model)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Simüle edilen iletişim mesajı: {Email} - {Subject}", model.Email, model.Subject);
            TempData["SuccessMessage"] = "Mesajınız (simüle olarak) alındı. Veritabanı henüz bağlanmadı.";
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    // Hızlı iletişim formu (AJAX - şimdilik sadece simüle)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult QuickContact([FromBody] ContactMessage model)
    {
        if (ModelState.IsValid)
        {
            _logger.LogInformation("Simüle hızlı iletişim mesajı: {Email}", model.Email);
            return Json(new { success = true, message = "Mesajınız alındı (simüle)." });
        }

        return Json(new { success = false, message = "Lütfen tüm alanları doğru şekilde doldurunuz." });
    }

    // İletişim bilgileri
    public IActionResult Info()
    {
        return View();
    }

    // Harita sayfası
    public IActionResult Map()
    {
        return View();
    }

    // SSS (Sıkça Sorulan Sorular)
    public IActionResult Faq()
    {
        return View();
    }

    // Teknik destek
    public IActionResult Support()
    {
        return View();
    }
}
