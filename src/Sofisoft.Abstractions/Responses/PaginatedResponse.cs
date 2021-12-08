using System.Collections.Generic;

namespace Sofisoft.Abstractions.Responses
{
    /// <summary>
    /// Pagination model for listings.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class PaginatedResponse<TResponse> where TResponse : class
    {
        /// <summary>
        /// Gets the start record.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Gets the number of records returned by the list.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Get the number of total records.
        /// </summary>
        public long Total { get; }

        /// <summary>
        /// Gets enumerable from the records.
        /// </summary>
        public IEnumerable<TResponse> Data { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="PaginatedResponse"/>.
        /// </summary>
        /// <param name="start">The start record.</param>
        /// <param name="pageSize">The number of records returned by the list.</param>
        /// <param name="total">The number of total records.</param>
        /// <param name="data">Data to be returned in the pagination.</param>
        public PaginatedResponse(int start, int pageSize, long total, IEnumerable<TResponse> data)
        {
            Start = start;
            PageSize = pageSize;
            Total = total;
            Data = data;
        }
    }
}