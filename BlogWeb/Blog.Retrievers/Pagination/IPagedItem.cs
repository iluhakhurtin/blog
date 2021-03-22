namespace Blog.Retrievers.Pagination
{
    public interface IPagedItem
    {
        int TotalResultsCount { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }

        public int TotalPagesCount { get; }
    }
}
