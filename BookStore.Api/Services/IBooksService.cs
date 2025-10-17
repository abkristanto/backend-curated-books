using System;
using BookStore.Api.Models;

namespace BookStore.Api.Services;

public interface IBooksService
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default);
    Task<Book?> GetAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(Book book, CancellationToken ct = default);

    Task<bool> UpdateAsync(Book book, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
