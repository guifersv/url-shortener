using System.Text;
using UrlShortener.Application.Dtos;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Utilities;

public static class Utils
{
    public static ShortUrlDto ShortUrlToDto(ShortUrlModel shortUrlModel)
    {
        return new()
        {
            Url = shortUrlModel.Url,
            Alias = shortUrlModel.Alias,
            Accesses = shortUrlModel.Accesses,
        };
    }

    public static string CreateAlias()
    {
        Random random = new();
        const string allowedChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var aliasRange = random.Next(5, 8);
        StringBuilder output = new();

        for (int i = 0; i != aliasRange; i++)
        {
            output.Append(allowedChars[random.Next(0, allowedChars.Length - 1)]);
        }
        return output.ToString();
    }
}
