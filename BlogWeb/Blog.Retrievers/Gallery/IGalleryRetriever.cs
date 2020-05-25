using System;
using System.Threading.Tasks;

namespace Blog.Retrievers.Gallery
{
    public interface IGalleryRetriever : IRetriever
    {
        Task<GalleryPagedDataTable> GetGalleryPagedAsync(
            string smallFileNameFilter,
            string previewFileNameFilter,
            string originalFileNameFilter,
            string articleTitleFilter,
            string descriptionFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize);
    }
}
