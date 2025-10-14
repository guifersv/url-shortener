using UrlShortener.Domain;
using UrlShortener.Services.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UrlShortener.Pages;

public class IndexModel(IUrlShortenerService service, ILogger<IndexModel> logger) : PageModel
{
    private readonly ILogger<IndexModel> _logger = logger;
    private readonly IUrlShortenerService _service = service;

    public void OnGet()
    {

    }
}