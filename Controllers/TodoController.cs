using Microsoft.AspNetCore.Mvc;
using ToDoApi.DTOs;


[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;

    public TodoController(ITodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var todos = await _service.GetAllAsync();

        var response = todos.Select(x => new TodoResponseDto
        {
            Id = x.Id,
            Title = x.Title,
            IsCompleted = x.IsCompleted
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await _service.GetByIdAsync(id);

        if (todo == null)
            return NotFound();

        var response = new TodoResponseDto
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoCreateDto dto)
    {
        var item = await _service.CreateAsync(dto.Title);

        var response = new TodoResponseDto
        {
            Id = item.Id,
            Title = item.Title,
            IsCompleted = item.IsCompleted
        };

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TodoResponseDto dto)
    {
        var result = await _service.UpdateAsync(id, dto.Title, dto.IsCompleted);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}