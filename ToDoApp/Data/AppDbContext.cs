using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;
using ToDoApp.DTOs;

namespace ToDoApp.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();

public DbSet<ToDoApp.DTOs.ToDoItemDTO> ToDoItemDTO { get; set; } = default!;
}
