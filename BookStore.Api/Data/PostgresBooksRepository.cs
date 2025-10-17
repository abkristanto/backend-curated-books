using BookStore.Api.Models;
using Npgsql;
using System;

namespace BookStore.Api.Data;

public class PostgresBooksRepository : IBooksRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PostgresBooksRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default)
    {
        const string sql = "SELECT id, title, author, genre, price, published_on FROM books ORDER BY id";
        var results = new List<Book>();

        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);
        await using var reader = await cmd.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            results.Add(new Book
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Author = reader.GetString(2),
                Genre = reader.IsDBNull(3) ? null : reader.GetString(3),
                Price = reader.GetDecimal(4),
                PublishedOn = DateOnly.FromDateTime(reader.GetDateTime(5))
            }
            );
        }
        return results;
    }

    public async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        const string sql = "SELECT id, title, author, genre, price, published_on FROM books WHERE id = @id";

        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await using var reader = await cmd.ExecuteReaderAsync(ct);
        if (!await reader.ReadAsync(ct)) return null;

        return new Book
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Author = reader.GetString(2),
            Genre = reader.IsDBNull(3) ? null : reader.GetString(3),
            Price = reader.GetDecimal(4),
            PublishedOn = DateOnly.FromDateTime(reader.GetDateTime(5))
        };
    }

    public async Task<int> CreateAsync(Book book, CancellationToken ct = default)
    {
        const string sql = @"
            INSERT INTO books (title, author, genre, price, published_on)
            VALUES (@title, @author, @genre, @price, @published_on)
            RETURNING id;";

        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@title", book.Title);
        cmd.Parameters.AddWithValue("@author", book.Author);
        cmd.Parameters.AddWithValue("@genre", (object?)book.Genre ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@price", book.Price);
        cmd.Parameters.AddWithValue("@published_on", book.PublishedOn.ToDateTime(TimeOnly.MinValue));

        var id = (int)(await cmd.ExecuteScalarAsync(ct))!;
        return id;
    }

    public async Task<bool> UpdateAsync(Book book, CancellationToken ct = default)
    {
        const string sql = @"
            UPDATE books
            SET title=@title, author=@author,
                genre=@genre, price=@price, published_on=@published_on
            WHERE id=@id";

        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", book.Id);
        cmd.Parameters.AddWithValue("@title", book.Title);
        cmd.Parameters.AddWithValue("@author", book.Author);
        cmd.Parameters.AddWithValue("@genre", (object?)book.Genre ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@price", book.Price);
        cmd.Parameters.AddWithValue("@published_on", book.PublishedOn.ToDateTime(TimeOnly.MinValue));

        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 1;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        const string sql = "DELETE FROM books WHERE id = @id";

        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        var rows = await cmd.ExecuteNonQueryAsync(ct);
        return rows == 1;
    }
}
