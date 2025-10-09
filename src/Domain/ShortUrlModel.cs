using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain;

public class ShortUrlModel
{
    public required string Url { get; init; }
    [MaxLength(30), Key]
    public required string Alias { get; init; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; init; }
}

public record ShortUrlDto
{
    [Url(ErrorMessage = "Invalid URL.")]
    public required string Url { get; set; }
    [StringLength(30, ErrorMessage = "The alias must be less than 30 characters.")]
    public required string Alias { get; set; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; set; }
}