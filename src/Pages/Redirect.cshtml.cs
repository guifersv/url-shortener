using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.Pages;

public class RedirectModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    public async Task<IActionResult> OnGetAsync(string alias)
    {
        var model = await _service.GetShortUrlModelByAlias(alias);

        if (model is not null)
        {
            _logger.LogInformation("RedirectModel: Redirecting to url.");
            await _service.IncrementShortUrlModelAccessCount(model.Alias!);
            return RedirectPermanent(model.Url);
        }

        _logger.LogWarning("RedirectModel: The model with does not exist in the database.");
        return NotFound();
    }
}
