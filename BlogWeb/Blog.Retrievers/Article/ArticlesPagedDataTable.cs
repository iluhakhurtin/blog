using System;
using System.Data;

namespace Blog.Retrievers.Article
{
    public class ArticlesPagedDataTable : PagedDataTable
    {
        public const string Title = "Title";
        public const string Body = "Body";
        public const string Timestamp = "Timestamp";
        public const string Roles = "Roles";
        public const string Categories = "Categories";

        public ArticlesPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(Title, typeof(string));
            base.Columns.Add(column);

            //column = new DataColumn(Body, typeof(string));
            //base.Columns.Add(column);

            column = new DataColumn(Timestamp, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Roles, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Categories, typeof(string));
            base.Columns.Add(column);
        }
    }
}
