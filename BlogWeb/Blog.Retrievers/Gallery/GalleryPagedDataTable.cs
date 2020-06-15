using System;
using System.Data;

namespace Blog.Retrievers.Gallery
{
    public class GalleryPagedDataTable : PagedDataTable
    {
        public const string SmallFileId = "SmallFileId";
        public const string SmallFileName = "SmallFileName";
        public const string ImageId = "ImageId";
        public const string PreviewFileId = "PreviewFileId";
        public const string PreviewFileName = "PreviewFileName";
        public const string OriginalFileId = "OriginalFileId";
        public const string OriginalFileName = "OriginalFileName";
        public const string ArticleId = "ArticleId";
        public const string ArticleTitle = "ArticleTitle";
        public const string Description = "Description";
        public const string Timestamp = "Timestamp";

        public GalleryPagedDataTable(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            DataColumn column = new DataColumn(Id, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(SmallFileId, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(SmallFileName, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(ImageId, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(PreviewFileId, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(PreviewFileName, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(OriginalFileId, typeof(Guid));
            base.Columns.Add(column);

            column = new DataColumn(OriginalFileName, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(ArticleId, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(ArticleTitle, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Description, typeof(string));
            base.Columns.Add(column);

            column = new DataColumn(Timestamp, typeof(string));
            base.Columns.Add(column);
        }
    }
}
