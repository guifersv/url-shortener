using UrlShortener.Domain;

namespace UrlShortener.Utilities;

public static class Utils
{
    public static ShortUrlDto ShortUrlModelToDto(ShortUrlModel shortUrlModel)
    {
        ShortUrlDto shortUrlDto = new()
        {
            Url = shortUrlModel.Url,
            Alias = shortUrlModel.Alias,
            Accesses = shortUrlModel.Accesses,
        };
        return shortUrlDto;
    }

    public static string CreateAlias()
    {
        Random random = new();
        var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        var aliasRange = random.Next(5, 8);
        char[] chars = new char[aliasRange];

        for (int i = 0; i < aliasRange; i++)
        {
            var currentChar = allowedChars[random.Next(0, allowedChars.Length - 1)];
            chars[i] = currentChar;
        }
        return new string(chars);
    }
}
