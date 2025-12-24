using UrlShortener.Application.Dtos;

namespace UrlShortener.Application.Services.Interfaces;

public interface IUrlShortenerService
{
    public Task<ShortUrlDto> CreateShortUrlModel(ShortUrlDto shortUrlDto);
    public Task<ShortUrlDto?> GetShortUrlModelByAlias(string alias);
    public Task<bool> DeleteShortUrlModel(string alias);
    public Task<bool> IncrementShortUrlModelAccessCount(string alias);
    public Task<IEnumerable<ShortUrlDto>> GetAllShortUrls();
}