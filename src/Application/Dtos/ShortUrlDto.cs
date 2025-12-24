using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace UrlShortener.Application.Dtos;

public record ShortUrlDto
{
    [StringLength(
        25,
        MinimumLength = 6,
        ErrorMessage = "The alias must be greater than 6 and less than 25 characters."
    )]
    public string? Alias { get; set; }

    [Url(ErrorMessage = "Invalid URL.")]
    public required string Url { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenReading)]
    public int Accesses { get; set; }
}
