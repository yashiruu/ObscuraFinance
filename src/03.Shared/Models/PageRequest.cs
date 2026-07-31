namespace Obscura.FinanceTracker.Shared.Models
{
    /// <summary>
    /// Query parameters for requesting a page of data. Bound from the query string.
    /// </summary>
    public class PagedRequest
    {
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;
        private int _pageNumber = 1;
        private int _pageSize = DefaultPageSize;

        /// <summary>
        /// Page number to retrieve. Values below 1 are clamped to 1. Default: 1.
        /// </summary>
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = (value < 1) ? 1 : value;
        }

        /// <summary>
        /// Items per page. Values below 1 are clamped to the default (10); values above 50 are clamped to 50.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value < 1) ? DefaultPageSize : (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}