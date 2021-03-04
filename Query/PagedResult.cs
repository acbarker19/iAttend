using System.Collections.Generic;

namespace iAttend.Query
{
    public class PagedResult<T>
    {
        public int TotalCount { get; }
        public int FilteredCount { get; }
        public List<T> Records { get; }

        public PagedResult(IEnumerable<T> records, int totalCount, int filteredCount)
        {
            TotalCount = totalCount;
            FilteredCount = filteredCount;
            Records = new List<T>(records ?? new List<T>());
        }
    }
}