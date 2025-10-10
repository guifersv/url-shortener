using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain;

public class ShortUrlModel
{
    public int Id { get; set; }
    public required string Url { get; init; }
    [MaxLength(30), MinLength(5)]
    public required string Alias { get; set; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; init; }
}

public record ShortUrlDto
{
    [Url(ErrorMessage = "Invalid URL.")]
    public required string Url { get; set; }
    [StringLength(30, MinimumLength = 5, ErrorMessage = "The alias must be greater that 5 and less than 30 characters.")]
    public required string Alias { get; set; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; set; }
}