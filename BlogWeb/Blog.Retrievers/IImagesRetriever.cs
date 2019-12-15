using System;
using System.Threading.Tasks;

namespace Blog.Retrievers
{
    public interface IImagesRetriever : IRetriever
    {
        Task<byte[]> GetPreviewFileDataAsync(Guid imageId);
        Task<byte[]> GetOriginalFileDataAsync(Guid imageId);
    }
}
