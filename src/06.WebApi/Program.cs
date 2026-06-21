using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;
using Obscura.FinanceTracker.Infrastructure.Services;
using Obscura.FinanceTracker.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

/// Register Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// Dependency Injection
/// Register the AppDbContext with the dependency injection container
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

/// Register Services and Interfaces
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migrate the database and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    var resetDatabase = builder.Configuration.GetValue<bool>("SeederOptions:ResetDatabase");

    var shouldResetDatabase = resetDatabase && app.Environment.IsDevelopment();

    await DataSeeder.SeedAsync(dbContext, shouldResetDatabase);
}

app.UseHttpsRedirection();

/// Use custom exception handling middleware
app.UseExceptionMiddleware();

app.MapControllers();

app.Run();