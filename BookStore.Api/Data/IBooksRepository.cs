using System;
using BookStore.Api.Models;

namespace BookStore.Api.Data;

public interface IBooksRepository
{
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default);
    Task<Book?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CreateAsync(Book book, CancellationToken ct = default);
    Task<bool> UpdateAsync(Book book, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
