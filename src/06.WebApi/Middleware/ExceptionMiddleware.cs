using Obscura.FinanceTracker.Application.Common.Responses;
using FluentValidation;
using Obscura.FinanceTracker.Shared.Exceptions;

namespace Obscura.FinanceTracker.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            #region Request Validation
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse("Validation Failed", errors));
            }
            #endregion

            #region Business Validation
            catch (BusinessException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
            #endregion

            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    ApiResponse<object>.ErrorResponse("An unexpected error occurred."));
            }
        }
    }
}
