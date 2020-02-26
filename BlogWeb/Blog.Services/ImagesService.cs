using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;

namespace Blog.Services
{
    public interface IImagesService
    {
        Task<string> AddImage(string previewFileId, string originalFileId);
        Task<string> EditImage(string id, string previewFileId, string originalFileId);
    }

    public class ImagesService : Service, IImagesService
    {
        private readonly IImagesRepository imagesRepository;

        public ImagesService(IRepositories repositories)
        {
            this.imagesRepository = repositories.ImagesRepository;
        }

        public async Task<string> AddImage(string previewFileId, string originalFileId)
        {
            var previewFileIdGuid = Guid.Parse(previewFileId);
            var originalFileIdGuid = Guid.Parse(originalFileId);

            var image = new Image();
            image.OriginalFileId = originalFileIdGuid;
            image.PreviewFileId = previewFileIdGuid;

            await this.imagesRepository.AddAsync(image);

            return String.Empty;
        }

        public async Task<string> EditImage(string id, string previewFileId, string originalFileId)
        {
            var imageId = Guid.Parse(id);
            var previewFileIdGuid = Guid.Parse(previewFileId);
            var originalFileIdGuid = Guid.Parse(originalFileId);

            var image = await this.imagesRepository.GetAsync(imageId);
            image.OriginalFileId = originalFileIdGuid;
            image.PreviewFileId = previewFileIdGuid;

            await this.imagesRepository.AddAsync(image);

            return String.Empty;
        }
    }
}
