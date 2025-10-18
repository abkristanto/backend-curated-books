using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Models;

public sealed record Book
{
    public int Id { get; init; }
    public required string Title { get; init; }   
    public required string Author { get; init; }
    public string? Genre { get; init; }
    public decimal Price { get; init; }
    public DateOnly PublishedOn { get; init; }
    
}
