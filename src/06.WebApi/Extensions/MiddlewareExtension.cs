using Obscura.FinanceTracker.WebApi.Middleware;

namespace Obscura.FinanceTracker.WebApi.Extensions
{
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
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Registers <see cref="ExceptionMiddleware"/>, which intercepts unhandled exceptions thrown
        /// anywhere in the pipeline and returns a consistent <c>ApiResponse</c> error instead of exposing
        /// raw stack traces. Must be registered early in the pipeline (before routing/controllers) so it
        /// can catch exceptions from all subsequent middleware and controllers.
        /// </summary>
        public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
