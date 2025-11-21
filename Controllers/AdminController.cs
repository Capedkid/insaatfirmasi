using Microsoft.AspNetCore.Mvc;

namespace InsaatFirmasi.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

