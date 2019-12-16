using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Retrievers
{
    public interface IImagesRetriever : IRetriever
    {
        Task<dynamic> GetPreviewImageDataAsync(Guid imageId);
        Task<dynamic> GetOriginalImageDataAsync(Guid imageId);
    }
}
