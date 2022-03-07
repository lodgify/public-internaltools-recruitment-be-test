namespace SuperPanel.App.Models.Base.Pagination
{
    public interface IPagination
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }

    public class Pagination : IPagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Pagination() { }

        public Pagination(int page, int take)
        {
            PageNumber = page;
            PageSize = take;
        }
    }
}
