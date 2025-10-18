using System;
using BookStore.Api.Dtos;
using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BooksController : ControllerBase
{
    private readonly IBooksService _service;

    public BooksController(IBooksService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Book>>> GetAll(CancellationToken ct)
    {
        var books = await _service.GetAllAsync(ct);
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> Get(int id, CancellationToken ct)
    {
        var book = await _service.GetAsync(id, ct);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookDto dto, CancellationToken ct)
    {
        var book = new Book
        {
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            Genre = dto.Genre?.Trim(),
            Price = dto.Price!.Value,
            PublishedOn = DateOnly.FromDateTime(dto.PublishedOn!.Value)
        };

        var id = await _service.CreateAsync(book, ct);

        var created = await _service.GetAsync(id, ct);
        if (created is null) return Problem("Book creation failed");

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto, CancellationToken ct)
    {
        var book = new Book
        {
            Id = id,
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            Genre = dto.Genre?.Trim(),
            Price = dto.Price!.Value,
            PublishedOn = DateOnly.FromDateTime(dto.PublishedOn!.Value)
        };

        var success = await _service.UpdateAsync(book, ct);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var success = await _service.DeleteAsync(id, ct);
        return success ? NoContent() : NotFound();
    }
}
