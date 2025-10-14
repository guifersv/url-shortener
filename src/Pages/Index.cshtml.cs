using UrlShortener.Domain;
using UrlShortener.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortener.Pages;

public class IndexModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    public async Task<IActionResult> OnGetAsync(string alias)
    {
        _logger.LogInformation("IndexModel: Redirecting to url with alias: {alias}", alias);
        var model = await _service.FindShortUrlModelByAlias(alias);

        if (model is not null)
            return Redirect(model.Url);
        else
        {
            _logger.LogWarning("IndexModel: The model with alias: {alias} does not exist in the database", alias);
            return NotFound();
        }
    }
}