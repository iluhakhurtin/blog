using System;
using System.Collections.Generic;
using System.Data;

namespace Blog.Retrievers
{
    public abstract class PagedItemsList<T>
    {
        public List<T> Items { get; set; }
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

        protected PagedItemsList()
            : this(0, 0)
        {

        }

        public PagedItemsList(int pageNumber, int pageSize)
        {
            this.Items = new List<T>();
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
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
