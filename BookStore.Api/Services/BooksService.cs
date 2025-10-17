using System;
using BookStore.Api.Data;
using BookStore.Api.Models;

namespace BookStore.Api.Services;

public sealed class BooksService : IBooksService
{
    private readonly IBooksRepository _repo;

    public BooksService(IBooksRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default) => _repo.GetAllAsync(ct);

    public Task<Book?> GetAsync(int id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);

    public async Task<int> CreateAsync(Book book, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(book.Title)) throw new ArgumentException("Book must have a title");
        if (string.IsNullOrWhiteSpace(book.Author)) throw new ArgumentException("Book must have an author");

        if (book.Price <= 0) throw new ArgumentException("Book price must be greater than zero");

        var normalized = book with
        {
            Title = book.Title.Trim(),
            Author = book.Author.Trim(),
            Genre = book.Genre?.Trim()
        };

        return await _repo.CreateAsync(normalized, ct);
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(book.Id, ct);
        if (existing is null) return false;

        if (book.Price < 0) throw new ArgumentException("Book price cannot be negative");

        return await _repo.UpdateAsync(book, ct);
    }

    public Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    => _repo.DeleteAsync(id, ct);
}
