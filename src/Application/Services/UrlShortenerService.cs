using UrlShortener.Application.Dtos;
using UrlShortener.Application.Services.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Utilities;

namespace UrlShortener.Application.Services;

public class UrlShortenerService(
    IUrlShortenerRepository repository,
    ILogger<UrlShortenerService> logger
) : IUrlShortenerService
{
    private readonly IUrlShortenerRepository _repository = repository;
    private readonly ILogger<UrlShortenerService> _logger = logger;

    public async Task<ShortUrlDto> CreateShortUrlModel(ShortUrlDto shortUrlDto)
    {
        _logger.LogDebug("UrlShortenerService: Creating ShortUrl.");
        var alias = shortUrlDto.Alias;

        if (string.IsNullOrEmpty(alias))
        {
            do
            {
                alias = Utils.CreateAlias();
            } while (await _repository.GetShortUrlModelByAlias(alias) is not null);
        }

        ShortUrlModel shortUrlModel = new() { Url = shortUrlDto.Url, Alias = alias };
        var createdModel = await _repository.CreateShortUrlModel(shortUrlModel);
        return Utils.ShortUrlToDto(createdModel);
    }

    public async Task<ShortUrlDto?> GetShortUrlModelByAlias(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Retrieving ShortUrlModel by alias.");
        var shortUrlModel = await _repository.GetShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
            return Utils.ShortUrlToDto(shortUrlModel);

        _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
        return null;
    }

    public async Task<bool> DeleteShortUrlModel(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Deleting ShortUrlModel by alias.");
        var shortUrlModel = await _repository.FindShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
        {
            await _repository.DeleteShortUrlModel(shortUrlModel);
            return true;
        }

        _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
        return false;
    }

    public async Task<bool> IncrementShortUrlModelAccessCount(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Updating ShortUrlModel Access count with alias");
        var shortUrlModel = await _repository.FindShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
        {
            await _repository.IncrementShortUrlAccessCount(shortUrlModel);
            return true;
        }

        _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
        return false;
    }

    public async Task<IEnumerable<ShortUrlDto>> GetAllShortUrls()
    {
        _logger.LogDebug("UrlShortenerService: Retrieving ShortUrl models from the database.");
        return (await _repository.GetAllShortUrls())
            .Select(m => new ShortUrlDto()
            {
                Url = m.Url,
                Alias = m.Alias,
                Accesses = m.Accesses,
            })
            .ToList();
    }
}
