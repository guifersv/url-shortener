using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Domain;

[Index(nameof(Alias), IsUnique = true)]
public class ShortUrlModel
{
    public int Id { get; set; }
    public required string Url { get; init; }
    [Required]
    public string? Alias { get; set; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; init; }
}

public record ShortUrlDto
{
    [Url(ErrorMessage = "Invalid URL.")]
    public required string Url { get; set; }
    [StringLength(30, MinimumLength = 5, ErrorMessage = "The alias must be greater than 5 and less than 30 characters.")]
    public string? Alias { get; set; }
    public int Accesses { get; set; }
    public DateTime DateCreated { get; set; }
}