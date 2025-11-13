using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UrlShortener.Application.Dtos;

public record ShortUrlDto
{
    [Url(ErrorMessage = "Invalid URL.")]
    public required string Url { get; set; }

    [StringLength(
        25,
        MinimumLength = 6,
        ErrorMessage = "The alias must be greater than 6 and less than 25 characters."
    )]
    public string? Alias { get; set; }

    [BindNever]
    public int Accesses { get; set; }
}
