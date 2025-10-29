using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using UrlShortener.Services.Interfaces;

namespace UrlShortener.Pages;

public class RedirectModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    public async Task<IActionResult> OnGetAsync(string alias)
    {
        _logger.LogDebug("RedirectModel: Redirecting to url with alias: {alias}", alias);
        var model = await _service.FindShortUrlModelByAlias(alias);

        if (model is not null)
        {
            _logger.LogInformation("RedirectModel: Redirecting to url: {url}", model.Url);
            await _service.IncrementShortUrlAccessCount(model.Alias!);
            return RedirectPermanent(model.Url);
        }
        else
        {
            _logger.LogWarning(
                "RedirectModel: The model with alias: {alias} does not exist in the database",
                alias
            );
            return NotFound();
        }
    }
}