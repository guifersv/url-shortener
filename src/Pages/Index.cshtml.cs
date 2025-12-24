using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrlShortener.Application.Dtos;
using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Pages;

public class IndexModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    [BindProperty]
    public required ShortUrlDto ShortUrl { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogDebug("IndexModel: Creating Model with provided form.");
        if (!ModelState.IsValid)
            return Page();

        if (
            string.IsNullOrWhiteSpace(ShortUrl.Alias)
            || await _service.GetShortUrlModelByAlias(ShortUrl.Alias) is null
        )
        {
            await _service.CreateShortUrlModel(ShortUrl);
            return RedirectToPage();
        }

        _logger.LogWarning("IndexModel: alias is not available.");

        ModelState.AddModelError("ShortUrl.Alias", "Alias is not available.");
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string alias)
    {
        _logger.LogInformation("IndexModel: Deleting Model.");

        var result = await _service.DeleteShortUrlModel(alias);

        return result ? RedirectToPage("Index") : BadRequest();
    }
}
