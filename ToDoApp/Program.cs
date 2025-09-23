using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ToDoApp;
using ToDoApp.Data;
using ToDoApp.DTOs.Auth;
using ToDoApp.DTOs.Validation;
using ToDoApp.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AuthenticationAndAuthorization(builder.Configuration);
builder.Services.AddSwagger();



builder.Services.AddScoped<IToDoService, ToDoService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("TODO_DBContext"));
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidotor>();
builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.EnablePersistAuthorization());
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
