using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.ViewComponents;

public class HeaderCategoriesViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public HeaderCategoriesViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.Name)
            .Take(8)
            .ToListAsync();

        return View(categories);
    }
}


