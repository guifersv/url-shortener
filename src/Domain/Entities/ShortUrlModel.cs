using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain.Entities;

public class ShortUrlModel
{
    [Key]
    [MaxLength(25)]
    public required string Alias { get; set; }

    public required string Url { get; set; }

    public int Accesses { get; set; }
}
