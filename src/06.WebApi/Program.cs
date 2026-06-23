using Obscura.FinanceTracker.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// FRAMEWORK SERVICES
// =============================================================================
// Core ASP.NET Core services required for the Web API to function.
// AddControllers() enables attribute-based routing and controller discoveRGry.
// AddEndpointsApiExplorer() + AddSwaggerGen() enable the Swagger UI for
// exploring and testing API endpoints during development.
// =============================================================================
builder.Services.AddControllers();
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddSwaggerDocumentation();

// DATABASE CONFIGURATION
builder.Services.AddDatabase(builder.Configuration);

// APPLICATION SERVICES
builder.Services.AddApplicationServices();

// =============================================================================
// BUILD APPLICATION
// =============================================================================
// Finalizes the service registration and produces the WebApplication instance.
// After this point, no more services can be added to the container.
// =============================================================================
var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseApplicationMiddleware();
app.UseHttpsRedirection();

// DATABASE INITIALIZATION
await app.InitializeDatabaseAsync();

// =============================================================================
// ROUTE REGISTRATION
// =============================================================================
// Map controller routes so that incoming HTTP requests are dispatched to the
// correct controller actions based on route attributes.
// =============================================================================
app.MapControllers();

app.Run();