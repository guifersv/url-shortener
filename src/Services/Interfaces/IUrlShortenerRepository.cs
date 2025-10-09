using UrlShortener.Domain;

namespace UrlShortener.Services.Interfaces;

public interface IUrlShortenerRepository
{
    public Task CreateShortUrlModel(ShortUrlModel shortUrlModel);
    public Task FindShortUrlModelByAlias(string alias);
    public Task DeleteShortUrlModel(string alias);
    public Task IncrementShortUrlAccessCount(string alias);
}