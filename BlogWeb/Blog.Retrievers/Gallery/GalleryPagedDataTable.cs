using System;
using System.Data;

namespace Blog.Retrievers.Gallery
{
    public class GalleryPagedDataTable : PagedDataTable
    {
        public const string SmallFileId = "SmallFileId";
        public const string SmallFileName = "SmallFileName";
        public const string PreviewFileId = "PreviewFileId";
        public const string PreviewFileName = "PreviewFileName";
        public const string OriginalFileId = "OriginalFileId";
        public const string OriginalFileName = "OriginalFileName";

        public GalleryPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(SmallFileId, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(SmallFileName, typeof(string));
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
