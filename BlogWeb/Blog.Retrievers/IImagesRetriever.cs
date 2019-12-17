using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Retrievers
{
    public interface IImagesRetriever : IRetriever
    {
        Task<ImageDataResult> GetPreviewImageDataAsync(Guid imageId);
        Task<ImageDataResult> GetOriginalImageDataAsync(Guid imageId);

        public class ImageDataResult
        {
            public byte[] Data { get; set; }
            public String MimeType { get; set; }
        }
    }
}
