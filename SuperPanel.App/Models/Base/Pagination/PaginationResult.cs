using SuperPanel.App.Models.DTO.Base;

namespace SuperPanel.App.Models.Base.Pagination
{
    public class PaginationResult : ResultBase
    {
        public int CurrentPage { get; set; }
        public bool IsLastPage { get; set; }
        public int TotalPage { get; set; }
    }
}
