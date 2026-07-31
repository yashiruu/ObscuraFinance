namespace Obscura.FinanceTracker.Application.Common.Responses
{
    /// <summary>
    /// Standard response envelope returned by every API endpoint, for both success and error outcomes.
    /// </summary>
    /// <typeparam name="T">The type of the payload carried in <see cref="Data"/>.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>Whether the request succeeded.</summary>
        public bool Success { get; set; }

        /// <summary>A human-readable summary of the outcome.</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>The response payload. <see langword="null"/> on error.</summary>
        public T? Data { get; set; }

        /// <summary>UTC timestamp at which the response was generated.</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Detailed error messages (e.g. validation failures). <see langword="null"/> on success.</summary>
        public List<string>? Errors { get; set; }

        /// <summary>
        /// Correlation id for the request (<c>HttpContext.TraceIdentifier</c>), set on error responses to help
        /// correlate a client-reported issue with server-side logs.
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// Builds a successful response.
        /// </summary>
        /// <param name="data">The payload to return.</param>
        /// <param name="message">A human-readable success message. Default: "Success".</param>
        public static ApiResponse<T> SuccessResponse(T data, string message = "Success")
        {
            return new ApiResponse<T>
            {
                Timestamp = DateTime.UtcNow,
                Success = true,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        /// <summary>
        /// Builds an error response.
        /// </summary>
        /// <param name="message">A human-readable error summary.</param>
        /// <param name="errors">Optional list of detailed error messages.</param>
        public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
        {
            return new ApiResponse<T>
            {
                Timestamp = DateTime.UtcNow,
                Success = false,
                Message = message,
                Data = default,
                Errors = errors
            };
        }
    }
}
