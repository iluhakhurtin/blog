using System;
using System.Data;

namespace Blog.Retrievers.Image
{
    public class ImagesPagedDataTable : PagedDataTable
    {
        public const string PreviewFileId = "PreviewFileId";
        public const string PreviewFileName = "PreviewFileName";
        public const string OriginalFileId = "OriginalFileId";
        public const string OriginalFileName = "OriginalFileName";

        public ImagesPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(PreviewFileId, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(PreviewFileName, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(OriginalFileId, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(OriginalFileName, typeof(string));
            base.Columns.Add(column);
        }
    }
}
