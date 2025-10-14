using UrlShortener.Domain;

namespace UrlShortener.Services.Interfaces;

public interface IUrlShortenerRepository
{
    public Task<ShortUrlModel> CreateShortUrlModel(ShortUrlModel shortUrlModel);
    public Task<ShortUrlModel?> FindShortUrlModelByAlias(string alias);
    public Task DeleteShortUrlModel(ShortUrlModel shortUrlModel);
    public Task<ShortUrlModel> IncrementShortUrlAccessCount(ShortUrlModel shortUrlModel);
    public Task<IEnumerable<ShortUrlModel>> GetAllShortUrls();
}