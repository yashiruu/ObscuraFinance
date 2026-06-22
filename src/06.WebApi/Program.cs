using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Application.Interfaces;
using Obscura.FinanceTracker.Infrastructure.Persistence;
using Obscura.FinanceTracker.Infrastructure.Persistence.Seeders;
using Obscura.FinanceTracker.Infrastructure.Services;
using Obscura.FinanceTracker.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// FRAMEWORK SERVICES
// =============================================================================
// Core ASP.NET Core services required for the Web API to function.
// AddControllers() enables attribute-based routing and controller discovery.
// AddEndpointsApiExplorer() + AddSwaggerGen() enable the Swagger UI for
// exploring and testing API endpoints during development.
// =============================================================================
builder.Services.AddControllers();
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);

        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================================================================
// DATABASE CONFIGURATION
// =============================================================================
// Register AppDbContext with the dependency injection container.
// EF Core manages the DbContext lifetime — one instance is created per HTTP
// request (Scoped lifetime) and disposed when the request ends.
//
// The connection string is read from appsettings.json under:
//   "ConnectionStrings": { "DefaultConnection": "..." }
// =============================================================================
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// =============================================================================
// APPLICATION SERVICES
// =============================================================================
// Register application services with Scoped lifetime.
// Scoped means one instance is created per HTTP request and shared across
// all components that resolve it within that same request.
//
// Controllers depend on abstractions (interfaces), not concrete implementations.
// This follows the Dependency Inversion Principle (DIP) from SOLID —
// high-level modules should not depend on low-level modules; both should
// depend on abstractions.
//
// Example resolution chain:
//   Controller → ICategoryService → CategoryService → AppDbContext
// =============================================================================
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// =============================================================================
// BUILD APPLICATION
// =============================================================================
// Finalizes the service registration and produces the WebApplication instance.
// After this point, no more services can be added to the container.
// =============================================================================
var app = builder.Build();


// =============================================================================
// MIDDLEWARE PIPELINE
// =============================================================================
// Middleware runs in the order it is registered. Each middleware component
// can process an incoming request, pass it to the next component, and then
// process the outgoing response on the way back.
//
// Request flow:
//   Swagger → ExceptionHandler → HTTPS Redirect → Controllers
// =============================================================================

// Swagger is only enabled in Development to avoid exposing API documentation
// in staging or production environments.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Register the global exception handling middleware.
// This intercepts unhandled exceptions thrown anywhere in the pipeline and
// returns a consistent error response instead of exposing raw stack traces.
// Must be registered early in the pipeline to catch exceptions from all
// subsequent middleware and controllers.
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

// =============================================================================
// DATABASE INITIALIZATION
// =============================================================================
// Apply pending EF Core migrations and seed initial data at startup.
//
// This runs inside a scoped service scope to correctly resolve AppDbContext,
// which has a Scoped lifetime. Creating a manual scope here is necessary
// because the application host itself operates at Singleton scope — resolving
// Scoped services directly from the root provider would cause a runtime error.
//
// Behavior by environment:
//   Development  → Migrations applied + optional database reset + seed data
//   Production   → Migrations applied only (reset is disabled for safety)
//
// The ResetDatabase flag is controlled via appsettings.json:
//   "SeederOptions": { "ResetDatabase": true }
// =============================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();

    var resetDatabase = builder.Configuration.GetValue<bool>("SeederOptions:ResetDatabase");

    var shouldResetDatabase = resetDatabase && app.Environment.IsDevelopment();

    await DataSeeder.SeedAsync(dbContext, shouldResetDatabase);
}

// =============================================================================
// ROUTE REGISTRATION
// =============================================================================
// Map controller routes so that incoming HTTP requests are dispatched to the
// correct controller actions based on route attributes.
// =============================================================================
app.MapControllers();

app.Run();