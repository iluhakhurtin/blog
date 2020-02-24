using System;
using System.Data;

namespace Blog.Retrievers.Article
{
    public class FilesPagedDataTable : PagedDataTable
    {
        public const string Name = "Name";
        public const string Extension = "Extension";
        public const string MimeType = "MimeType";

        public FilesPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(Name, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Extension, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(MimeType, typeof(string));
            base.Columns.Add(column);
        }
    }
}
