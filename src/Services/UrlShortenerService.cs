using UrlShortener.Services.Interfaces;
using UrlShortener.Domain;
using UrlShortener.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace UrlShortener.Services;

public class UrlShortenerService(IUrlShortenerRepository repository, ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    private readonly IUrlShortenerRepository _repository = repository;
    private readonly ILogger<UrlShortenerService> _logger = logger;

    public async Task<ShortUrlDto?> FindShortUrlModelByAlias(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Retrieving ShortUrlModel by alias: {alias}.", alias);
        var shortUrlModel = await _repository.FindShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
        {
            var model = await _repository.IncrementShortUrlAccessCount(shortUrlModel);
            return Utils.ShortUrlModelToDto(model);
        }
        else
        {
            _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
            return null;
        }
    }

    public async Task DeleteShortUrlModel(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Deleting ShortUrlModel by alias: {alias}.", alias);
        var shortUrlModel = await _repository.FindShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
            await _repository.DeleteShortUrlModel(shortUrlModel);
        else
            _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
    }

    public async Task IncrementShortUrlAccessCount(string alias)
    {
        _logger.LogDebug("UrlShortenerService: Updating ShortUrlModel Access count with alias: {alias}.", alias);
        var shortUrlModel = await _repository.FindShortUrlModelByAlias(alias);

        if (shortUrlModel is not null)
            await _repository.IncrementShortUrlAccessCount(shortUrlModel);
        else
            _logger.LogWarning("UrlShortenerService: The Model doesn't exist in the database.");
    }

    public async Task<IEnumerable<ShortUrlDto>> GetAllShortUrls()
    {
        _logger.LogDebug("UrlShortenerService: Retrieving ShortUrl models from the database");
        return (await _repository.GetAllShortUrls()).Select(m => new ShortUrlDto()
        {
            Url = m.Url,
            Alias = m.Alias,
            Accesses = m.Accesses,
            DateCreated = m.DateCreated
        }).ToList();
    }

    public async Task<ShortUrlDto> CreateShortUrlModel(ShortUrlDto shortUrlDto)
    {
        ShortUrlModel shortUrlModel = new()
        {
            Url = shortUrlDto.Url,
            Alias = shortUrlDto.Alias,
            Accesses = shortUrlDto.Accesses,
            DateCreated = shortUrlDto.DateCreated,
        };
        var createdModel = await _repository.CreateShortUrlModel(shortUrlModel);
        return Utils.ShortUrlModelToDto(createdModel);
    }
}