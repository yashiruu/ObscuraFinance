using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;

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

// Apply pending migrations and seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    await DataSeeder.SeedAsync(dbContext);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();