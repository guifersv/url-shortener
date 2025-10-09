using UrlShortener.Domain;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Infrastructure;

public class UrlShortenerContext(DbContextOptions<UrlShortenerContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls { get; set; }
}