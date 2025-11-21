using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsaatFirmasi.Data;
using InsaatFirmasi.Models;

namespace InsaatFirmasi.Controllers;

public class BlogController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlogController> _logger;

    public BlogController(ApplicationDbContext context, ILogger<BlogController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Tüm blog yazılarını listele
    public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
    {
        var query = _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue)
            .OrderByDescending(b => b.PublishedDate);

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalCount = totalCount;

        return View(posts);
    }

    // Blog yazısı detayı
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var blogPost = await _context.BlogPosts
            .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

        if (blogPost == null)
        {
            return NotFound();
        }

        // Görüntülenme sayısını artır
        blogPost.ViewCount++;
        await _context.SaveChangesAsync();

        // Son yazılar
        var recentPosts = await _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue && b.Id != id)
            .OrderByDescending(b => b.PublishedDate)
            .Take(3)
            .ToListAsync();

        // Etiketlere göre ilgili yazılar
        var relatedPosts = new List<BlogPost>();
        if (!string.IsNullOrEmpty(blogPost.Tags))
        {
            var tags = blogPost.Tags
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();

            relatedPosts = await _context.BlogPosts
                .Where(b => b.IsActive && b.PublishedDate.HasValue && b.Id != id &&
                            b.Tags != null &&
                            tags.Any(tag => b.Tags!.Contains(tag)))
                .OrderByDescending(b => b.PublishedDate)
                .Take(3)
                .ToListAsync();
        }

        ViewBag.RecentPosts = recentPosts;
        ViewBag.RelatedPosts = relatedPosts;

        return View(blogPost);
    }

    // Kategoriye göre blog yazıları
    public async Task<IActionResult> Category(string? category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return RedirectToAction(nameof(Index));
        }

        var posts = await _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue &&
                        ((b.Category != null && b.Category.Contains(category)) ||
                         (b.Tags != null && b.Tags.Contains(category))))
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();

        ViewBag.Category = category;
        ViewBag.ResultsCount = posts.Count;

        return View(posts);
    }

    // Blog yazısı arama
    [HttpPost]
    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return RedirectToAction(nameof(Index));
        }

        var posts = await _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue &&
                        ((b.Title != null && b.Title.Contains(searchTerm)) ||
                         (b.Summary != null && b.Summary.Contains(searchTerm)) ||
                         (b.Content != null && b.Content.Contains(searchTerm)) ||
                         (b.Tags != null && b.Tags.Contains(searchTerm))))
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.ResultsCount = posts.Count;

        return View("Index", posts);
    }

    // Öne çıkan blog yazıları
    public async Task<IActionResult> Featured()
    {
        var featuredPosts = await _context.BlogPosts
            .Where(b => b.IsActive && b.IsFeatured && b.PublishedDate.HasValue)
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();

        return View(featuredPosts);
    }

    // RSS Feed
    public async Task<IActionResult> Rss()
    {
        var posts = await _context.BlogPosts
            .Where(b => b.IsActive && b.PublishedDate.HasValue)
            .OrderByDescending(b => b.PublishedDate)
            .Take(20)
            .ToListAsync();

        return View(posts);
    }
}

