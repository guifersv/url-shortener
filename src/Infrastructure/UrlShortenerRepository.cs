
using UrlShortener.Domain;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Infrastructure;

public class UrlShortenerRepository(UrlShortenerContext context) : IUrlShortenerRepository
{
    private readonly UrlShortenerContext _context = context;
    public async Task<ShortUrlModel> CreateShortUrlModel(ShortUrlModel shortUrlModel)
    {
        var createdModel = await _context.ShortUrls.AddAsync(shortUrlModel);
        await _context.SaveChangesAsync();

        return createdModel.Entity;
    }

    Task IUrlShortenerRepository.DeleteShortUrlModel(string alias)
    {
        throw new NotImplementedException();
    }

    Task IUrlShortenerRepository.FindShortUrlModelByAlias(string alias)
    {
        throw new NotImplementedException();
    }

    Task IUrlShortenerRepository.IncrementShortUrlAccessCount(string alias)
    {
        throw new NotImplementedException();
    }
}