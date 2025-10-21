using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain;

namespace UrlShortener.Infrastructure;

public class UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : DbContext(options)
{
    public DbSet<ShortUrlModel> ShortUrls { get; set; }
}
