using System.Collections.Generic;

namespace iAttend.Query
{
    public abstract class BaseQuery
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string Search { get; set; }
    }
}