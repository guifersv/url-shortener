using Microsoft.EntityFrameworkCore;

using UrlShortener.Domain;
using UrlShortener.Services.Interfaces;
using UrlShortener.Utilities;

namespace UrlShortener.Infrastructure;

public class UrlShortenerRepository(UrlShortenerContext context) : IUrlShortenerRepository
{
    private readonly UrlShortenerContext _context = context;

    public async Task<ShortUrlModel> CreateShortUrlModel(ShortUrlModel shortUrlModel)
    {
        var createdModel = await _context.ShortUrls.AddAsync(shortUrlModel);

        if (string.IsNullOrEmpty(createdModel.Entity.Alias))
        {
            while (true)
            {
                var alias = Utils.CreateAlias();
                if (_context.ShortUrls.FirstOrDefaultAsync(m => m.Alias == alias) is null)
                {
                    createdModel.Entity.Alias = Utils.CreateAlias();
                    break;
                }
            }
        }

        await _context.SaveChangesAsync();

        return createdModel.Entity;
    }

    public async Task DeleteShortUrlModel(ShortUrlModel shortUrlModel)
    {
        _context.Remove(shortUrlModel);
        await _context.SaveChangesAsync();
    }

    public async Task<ShortUrlModel?> FindShortUrlModelByAlias(string alias)
    {
        return await _context.ShortUrls.FirstOrDefaultAsync(m => m.Alias == alias);
    }

    public async Task IncrementShortUrlAccessCount(ShortUrlModel shortUrlModel)
    {
        shortUrlModel.Accesses += 1;
        _context.ShortUrls.Update(shortUrlModel);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ShortUrlModel>> GetAllShortUrls()
    {
        return await _context.ShortUrls.AsNoTracking().ToListAsync();
    }
}