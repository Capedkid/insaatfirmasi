using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.ViewComponents;

public class HeaderLogoViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public HeaderLogoViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var logo = await _context.SiteLogos
            .OrderByDescending(l => l.CreatedDate)
            .FirstOrDefaultAsync();

        return View(logo);
    }
}


