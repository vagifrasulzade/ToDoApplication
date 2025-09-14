using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDoApp;
using ToDoApp.Auth;
using ToDoApp.Data;
using ToDoApp.Models;
using ToDoApp.Services;
using ToDoApp.Services.Auth;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IJwtService, JwtService>();

var jwtConfig = new JwtConfig();
builder.Configuration.Bind("JWT", jwtConfig);
builder.Services.AddSingleton(jwtConfig);


builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();


builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSwagger();



builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TODO_DBContext"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x=>x.EnablePersistAuthorization());
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
