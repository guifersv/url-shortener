using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Interfaces;

public interface IUrlShortenerRepository
{
    public Task<ShortUrlModel> CreateShortUrlModel(ShortUrlModel shortUrlModel);
    public Task<ShortUrlModel?> FindShortUrlModelByAlias(string alias);
    public Task<ShortUrlModel?> GetShortUrlModelByAlias(string alias);
    public Task DeleteShortUrlModel(ShortUrlModel shortUrlModel);
    public Task IncrementShortUrlAccessCount(ShortUrlModel shortUrlModel);
    public Task<IEnumerable<ShortUrlModel>> GetAllShortUrls();
}
