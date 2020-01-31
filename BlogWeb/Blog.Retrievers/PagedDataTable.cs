using System;
using System.Data;

namespace Blog.Retrievers
{
    public class PagedDataTable : DataTable
    {
        public const string Id = "Id";

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

        public int GetTotalPagesCount()
        {
            if (this.PageSize < 1)
                this.PageSize = 1;

            int totalPages = (this.TotalResultsCount + this.PageSize - 1) / this.PageSize;
            return totalPages;
        }

        public PagedDataTable()
            : this(0, 0)
        {

        }

        public PagedDataTable(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
