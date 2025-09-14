using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.DTOs;
using ToDoApp.DTOs.Pagination;
using ToDoApp.Models;

namespace ToDoApp.Services;

public class ToDoService : IToDoService
{
    private readonly AppDbContext _context;
    
    public ToDoService(AppDbContext context)
    {
        _context = context;
    }

    public Task<ToDoItemDTO> ChangeToDoItemStatusAsync(int id, bool isCompleted)
    {
        var item = _context.ToDoItems.FirstOrDefault(t => t.Id == id);

        if (item is null) return null!;

        item.IsCompleted = isCompleted;
        item.UpdatedAt = DateTimeOffset.UtcNow;

        _context.SaveChanges();

        return Task.FromResult(ConvertToDoItemDTO(item));

    }

    public Task<ToDoItemDTO> CreateToDoAsync(ToDoItemCreateDTO request)
    {
        var item = new ToDoItem()
        {
            Text = request.Text,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsCompleted = false
        };
        _context.ToDoItems.Add(item);
        _context.SaveChanges();

        return Task.FromResult(ConvertToDoItemDTO(item));
    }

    public Task<ToDoItemDTO> GetToDoItemAsync(int id)
    {
        var item = _context.ToDoItems.FirstOrDefault(t => t.Id == id);

        return Task.FromResult(ConvertToDoItemDTO(item!));
    }

    public async Task<PaginationListDTO<ToDoItemDTO>> GetToDoItemsAsync(
        int page,
        int pageSize,
        string? search,
        bool? isCompleted,
        DateTimeOffset? from,
        DateTimeOffset? to) 
    {
        IQueryable<ToDoItem> query = _context.ToDoItems;

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item => item.Text.ToLower().Contains(search));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(item => item.IsCompleted == isCompleted);
        }

        //Date filtering

        if (from.HasValue)
        {

            query = query.Where(item => item.CreatedAt >= from);
            
        }
        if (to.HasValue)
        {
            query = query.Where(item => item.CreatedAt <= to);
        }




        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();


        return new PaginationListDTO<ToDoItemDTO>(
            items.Select(item => ConvertToDoItemDTO(item)),
            new PaginationMeta(page, pageSize, query.Count())
            );
    }

    private ToDoItemDTO ConvertToDoItemDTO(ToDoItem item)
    {
        ToDoItemDTO todoItem = new()
        {
            Id = item.Id,
            Text = item.Text,
            CreatedAt = item.CreatedAt,
            isCompleted = item.IsCompleted
        };
        return todoItem;
    }
}
