using System;
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
    public async Task<IActionResult> Create(Book book, CancellationToken ct)
    {
        var id = await _service.CreateAsync(book, ct);

        var created = await _service.GetAsync(id, ct);
        if (created is null) return Problem("Book creation failed");

        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Book book, CancellationToken ct)
    {
        if (id != book.Id)
            return BadRequest("ID in URL does not match ID in body");

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
