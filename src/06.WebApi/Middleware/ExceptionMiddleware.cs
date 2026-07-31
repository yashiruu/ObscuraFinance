using Obscura.FinanceTracker.Application.Common.Responses;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Obscura.FinanceTracker.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Obscura.FinanceTracker.WebApi.Middleware
{
    /// <summary>
    /// Global exception handling middleware. Catches every unhandled exception thrown by controllers and
    /// application services, logs it centrally, and formats it into a standardized <see cref="ApiResponse{T}"/>
    /// so that no raw stack trace or framework error shape ever reaches the client.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Invokes the middleware, forwarding the request down the pipeline and handling any exception it throws.
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Continue the pipeline
                await _next(context);
            }
            catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
            {
                // The client disconnected or cancelled the request. There is no one left to respond to,
                // so we only log it (at a low severity, since this is expected client behavior, not a fault).
                _logger.LogInformation("Request was aborted by the client. Path: {Path}", context.Request.Path);
            }
            catch (Exception ex)
            {
                // Log the exception centrally, with the correlation id, so we don't have to
                // duplicate _logger.LogError in every specific case.
                _logger.LogError(ex, "An exception occurred during request processing. TraceId: {TraceId}, Message: {Message}",
                    context.TraceIdentifier, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set the content type once
            context.Response.ContentType = "application/json";

            // Use modern C# switch expression to determine status code and response body
            var (statusCode, response) = exception switch
            {
                // Request Validation Errors (FluentValidation)
                ValidationException validationEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ErrorResponse(
                        "Validation Failed",
                        validationEx.Errors.Select(e => e.ErrorMessage).ToList())
                ),

                // Business Logic Errors (Custom Domain Exceptions)
                BusinessException businessEx => (
                    StatusCodes.Status400BadRequest,
                    ApiResponse<object>.ErrorResponse(businessEx.Message)
                ),

                // Custom Domain Resource Not Found
                NotFoundException customNotFoundEx => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.ErrorResponse(customNotFoundEx.Message)
                ),

                // System Resource Not Found (Fallback)
                KeyNotFoundException notFoundEx => (
                    StatusCodes.Status404NotFound,
                    ApiResponse<object>.ErrorResponse(notFoundEx.Message)
                ),

                // Authorization Issues
                UnauthorizedAccessException _ => (
                    StatusCodes.Status401Unauthorized,
                    ApiResponse<object>.ErrorResponse("You are not authorized to perform this action.")
                ),

                // Optimistic concurrency conflict (row changed/deleted between read and save)
                DbUpdateConcurrencyException _ => (
                    StatusCodes.Status409Conflict,
                    ApiResponse<object>.ErrorResponse("The record was modified or deleted by another process. Please reload and try again.")
                ),

                // Other persistence failures (e.g. constraint violations). The raw provider error is
                // never exposed to the client; it is only logged (above) for diagnostics.
                DbUpdateException _ => (
                    StatusCodes.Status409Conflict,
                    ApiResponse<object>.ErrorResponse("The request could not be saved because it conflicts with existing data.")
                ),

                // Catch-all for any other unhandled exceptions (System Exceptions, DB connections, etc.)
                // We intentionally do NOT expose the raw ex.Message to the client for security reasons,
                // except in Development where it helps local debugging.
                _ => (
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<object>.ErrorResponse(
                        "An unexpected error occurred. Please contact support if the issue persists.",
                        _environment.IsDevelopment() ? [exception.ToString()] : null)
                )
            };

            // Attach the correlation id so the client can reference this specific request when reporting issues.
            response.TraceId = context.TraceIdentifier;

            // Apply the determined status code and write the standardized JSON response
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
