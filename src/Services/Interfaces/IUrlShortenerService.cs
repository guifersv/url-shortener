using UrlShortener.Domain;

namespace UrlShortener.Services.Interfaces;

public interface IUrlShortenerService
{
    // public Task<ShortUrlDto> CreateShortUrlModel(ShortUrlDto shortUrlDto);
    public Task<ShortUrlDto?> FindShortUrlModelByAlias(string alias);
    // public Task DeleteShortUrlModel(string alias);
    // public Task IncrementShortUrlAccessCount(string alias);
}