using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;
using ToDoApp.DTOs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ToDoApp.Data;

public class AppDbContext:IdentityDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {}

    public DbSet<ToDoItem> ToDoItems 
                        => Set<ToDoItem>();

    public DbSet<AppUser> AppUsers 
                        => Set<AppUser>();

}
