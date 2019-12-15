using System;
using System.Threading.Tasks;

namespace Blog.Retrievers.PostgreSQL
{
    internal class ImagesRetriever : Retriever, IImagesRetriever
    {
        public ImagesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public Task<byte[]> GetOriginalFileDataAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetPreviewFileDataAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }
    }
}
