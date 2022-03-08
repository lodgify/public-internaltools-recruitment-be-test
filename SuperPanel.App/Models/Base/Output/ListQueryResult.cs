using SuperPanel.App.Models.Base.Pagination;
using System.Collections.Generic;

namespace SuperPanel.App.Models.Base.Output
{
    public class ListQueryResult<TEntity> : PaginationResult
    {
        public IEnumerable<TEntity> Entities { get; set; }
    }
}
