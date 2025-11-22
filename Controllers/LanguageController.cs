using Microsoft.AspNetCore.Mvc;

namespace InsaatFirmasi.Controllers;

public class LanguageController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Set(string lang, string returnUrl)
    {
        if (lang != "tr" && lang != "en")
        {
            lang = "tr";
        }

        Response.Cookies.Append("lang", lang, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddYears(1),
            IsEssential = true,
            HttpOnly = false
        });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}


