using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using UrlShortener.Domain;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Pages;

public class DeleteModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    public required ShortUrlDto ShortUrl { get; set; }
    public async Task<IActionResult> OnGetAsync(string alias)
    {
        var model = await _service.FindShortUrlModelByAlias(alias);
        if (model is null)
            return NotFound();

        ShortUrl = model;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string alias)
    {
        _logger.LogDebug("IndexModel: Deleting Model: {alias}.", alias);
        if (!ModelState.IsValid)
            return Page();

        await _service.DeleteShortUrlModel(alias);
        return RedirectToPage("Index");
    }
}

