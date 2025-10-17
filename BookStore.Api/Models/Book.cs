using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.Models;

public class Book
{
    public int Id { get; set; }
    [Required, StringLength(200)] public required string Title { get; set; }   
    [Required, StringLength(200)] public required string Author { get; set; }
    [StringLength(80)] public string? Genre { get; set; }
    [Range(0, 1000)] public decimal Price { get; set; }
    [DataType(DataType.Date)] public DateOnly PublishedOn { get; set; }


    
}
