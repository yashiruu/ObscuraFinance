using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

/// Register Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// Dependency Injection
/// Register the AppDbContext with the dependency injection container
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();