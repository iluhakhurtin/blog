using System;
using System.Threading.Tasks;
using Blog.Retrievers.Article;

namespace Blog.Retrievers.File
{
    public interface IFilesRetriever : IRetriever
    {
        Task<FilesPagedDataTable> GetFilesPagedAsync(
            string nameFilter,
            string extensionFilter,
            string mimeTypeFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize);
    }
}
