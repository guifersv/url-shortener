using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Services.Interfaces;

namespace UrlShortener.ViewComponents;

public class CreatedModelsViewComponent(IUrlShortenerService service) : ViewComponent
{
    private readonly IUrlShortenerService _service = service;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _service.GetAllShortUrls());
    }
}