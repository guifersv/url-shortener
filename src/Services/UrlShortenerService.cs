using UrlShortener.Services.Interfaces;
using UrlShortener.Domain;
using UrlShortener.Utilities;

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
            return Utils.ShortUrlModelToDTO(shortUrlModel);
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
}