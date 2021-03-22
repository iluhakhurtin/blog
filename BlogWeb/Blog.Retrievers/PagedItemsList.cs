using System.Collections.Generic;
using Blog.Retrievers.Pagination;

namespace Blog.Retrievers
{
    public abstract class PagedItemsList<T> : IPagedItem
    {
        private IPagedItem pagedItem;

        public List<T> Items { get; set; }

        #region IPagination Members

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

        protected PagedItemsList()
            : this(0, 0)
        {

        }

        public PagedItemsList(int pageNumber, int pageSize)
        {
            this.Items = new List<T>();

            this.pagedItem = new PagedItem(pageNumber, pageSize);
        }

        public void AddItem(T item)
        {
            this.Items.Add(item);
        }

        public void AddItems(IEnumerable<T> items)
        {
            this.Items.AddRange(items);
        }
    }
}
