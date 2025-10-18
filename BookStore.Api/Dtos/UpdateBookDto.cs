using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Dtos;

public record class UpdateBookDto
{
    [Required, StringLength(200)]
    public string Title { get; init; } = default!;

    [Required, StringLength(200)]
    public string Author { get; init; } = default!;

    [StringLength(80)]
    public string? Genre { get; init; }

    [Required, Range(0, 1000)]
    public decimal? Price { get; init; }

    [Required, DataType(DataType.Date)]
    public DateTime? PublishedOn { get; init; }
}
