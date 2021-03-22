using System.Data;
using Blog.Retrievers.Pagination;

namespace Blog.Retrievers
{
    public class PagedDataTable : DataTable, IPagedItem
    {
        private IPagedItem pagedItem;

        public const string Id = "Id";

        #region IPager Members

        public int TotalResultsCount
        {
            get
            {
                return this.pagedItem.TotalResultsCount;
            }

            set
            {
                this.pagedItem.TotalResultsCount = value;
            }
        }

        public int PageNumber
        {
            get
            {
                return this.pagedItem.PageNumber;
            }

            set
            {
                this.pagedItem.PageNumber = value;
            }
        }

        public int PageSize
        {
            get
            {
                return this.pagedItem.PageSize;
            }

            set
            {
                this.pagedItem.PageSize = value;
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return this.pagedItem.TotalPagesCount;
            }
        }

        #endregion

        public PagedDataTable()
            : this(0, 0)
        {

        }

        public PagedDataTable(int pageNumber, int pageSize)
        {
            this.pagedItem = new PagedItem(pageNumber, pageSize);
        }
    }
}
