using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain;

public class ShortUrl
{
    public int Id { get; set; }
    [Url]
    public required string Url { get; init; }
    [StringLength(30, MinimumLength = 5, ErrorMessage = "The alias must not be greater than 30 characters.")]
    public required string Alias { get; init; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; init; }
}