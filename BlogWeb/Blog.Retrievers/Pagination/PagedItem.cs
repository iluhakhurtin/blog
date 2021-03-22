namespace Blog.Retrievers.Pagination
{
    public class PagedItem : IPagedItem
    {
        public int TotalResultsCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPagesCount
        {
            get
            {
                return this.GetTotalPagesCount();
            }
        }

        private int GetTotalPagesCount()
        {
            if (this.PageSize < 1)
                this.PageSize = 1;

            int totalPages = (this.TotalResultsCount + this.PageSize - 1) / this.PageSize;
            return totalPages;
        }

        public PagedItem()
        {
        }

        public PagedItem(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
