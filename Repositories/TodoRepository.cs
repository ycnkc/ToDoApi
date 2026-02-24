using Microsoft.EntityFrameworkCore;
using ToDoApi.Data;
using ToDoApi.Models;
using ToDoApi.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public TodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItem>> GetAllAsync()
        => await _context.TodoItems.ToListAsync();

    public async Task<TodoItem?> GetByIdAsync(int id)
        => await _context.TodoItems.FindAsync(id);

    public async Task AddAsync(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TodoItem item)
    {
        _context.TodoItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem item)
    {
        _context.TodoItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}