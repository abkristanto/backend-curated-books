using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Models;

public sealed record Book
{
    public int Id { get; init; }
    [Required, StringLength(200)] public required string Title { get; init; }   
    [Required, StringLength(200)] public required string Author { get; init; }
    [StringLength(80)] public string? Genre { get; init; }
    [Range(0, 1000)] public decimal Price { get; init; }
    [DataType(DataType.Date)] public DateOnly PublishedOn { get; init; }
    
}
