using Microsoft.AspNetCore.Mvc;
using Obscura.FinanceTracker.Application.Common.Responses;
using Obscura.FinanceTracker.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// FRAMEWORK SERVICES
// =============================================================================
// Core ASP.NET Core services required for the Web API to function.
// AddControllers() enables attribute-based routing and controller discoveRGry.
// AddEndpointsApiExplorer() + AddSwaggerGen() enable the Swagger UI for
// exploring and testing API endpoints during development.
//
// ConfigureApiBehaviorOptions overrides the default automatic 400 response for
// invalid model state (e.g. malformed query/route/body values) so it uses the
// same ApiResponse<T> envelope as ExceptionMiddleware, instead of the framework's
// default ValidationProblemDetails shape.
// =============================================================================
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .SelectMany(entry => entry.Value!.Errors.Select(e => e.ErrorMessage))
                .ToList();

            var response = ApiResponse<object>.ErrorResponse("Validation Failed", errors);
            response.TraceId = context.HttpContext.TraceIdentifier;

            return new BadRequestObjectResult(response);
        };
    });
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddSwaggerDocumentation();

// DATABASE CONFIGURATION
builder.Services.AddInfrastructureServices(builder.Configuration);

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