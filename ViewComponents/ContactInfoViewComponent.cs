using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;

namespace InsaatFirmasi.ViewComponents;

public class ContactInfoViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ContactInfoViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
        return View(contactInfo);
    }
}

