using UrlShortener.Domain;

namespace UrlShortener.Utilities;

public static class Utils
{
    public static ShortUrlDto ShortUrlModelToDTO(ShortUrlModel shortUrlModel)
    {
        ShortUrlDto shortUrlDto = new()
        {
            Url = shortUrlModel.Url,
            Alias = shortUrlModel.Alias,
            Accesses = shortUrlModel.Accesses,
            DateCreated = shortUrlModel.DateCreated,
        };
        return shortUrlDto;
    }
}