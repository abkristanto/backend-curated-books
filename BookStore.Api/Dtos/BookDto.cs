namespace BookStore.Api.Dtos;

public record class BookDto(
    int Id,
    string Title,
    string Author,
    string? Genre,
    decimal Price,
    DateOnly PublishedOn
);
