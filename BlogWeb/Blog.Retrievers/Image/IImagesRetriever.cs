using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Retrievers.Image
{
    public interface IImagesRetriever : IRetriever
    {
        Task<ImageDataResult> GetPreviewImageDataAsync(Guid imageId);
        Task<ImageDataResult> GetOriginalImageDataAsync(Guid imageId);
        Task<ImageDataResult> GetPreviewImageDataByNameAsync(String fileName);
        Task<ImageDataResult> GetOriginalImageDataByNameAsync(String fileName);
    }
}
