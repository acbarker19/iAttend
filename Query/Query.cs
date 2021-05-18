using System.Collections.Generic;

namespace iAttend.Query
{
    public abstract class Query<TSort> : BaseQuery
    {
        public SortedList<int, (TSort SortBy, SortDirection Direction)> Sort { get; }
        
        protected Query()
        : base()
        {
            Sort = new SortedList<int, (TSort, SortDirection)>();
        }

        public Query<TSort> OrderBy(TSort sort)
        {
            Sort.Clear();
            
            return ThenBy(sort);
        }

        public Query<TSort> OrderByDescending(TSort sort)
        {
            Sort.Clear();
            
            return ThenByDescending(sort);
        }

        public Query<TSort> ThenBy(TSort sort)
        {
            Sort.Add(Sort.Count, (sort, SortDirection.Ascending));

            return this;
        }

        public Query<TSort> ThenByDescending(TSort sort)
        {
            Sort.Add(Sort.Count, (sort, SortDirection.Descending));

            return this;
        }
    }
}