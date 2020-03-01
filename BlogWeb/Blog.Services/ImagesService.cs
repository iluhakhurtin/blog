using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Blog.Services
{
    public interface IImagesService
    {
        Task<string> AddImage(string previewFileId, string originalFileId);
        Task<string> EditImage(string id, string previewFileId, string originalFileId);
        Task<byte[]> GetThumbnail(Guid imageFileId);
    }

    public class ImagesService : Service, IImagesService
    {
        private readonly IImagesRepository imagesRepository;
        private readonly IFilesRepository filesRepository;

        public ImagesService(IRepositories repositories)
        {
            this.imagesRepository = repositories.ImagesRepository;
            this.filesRepository = repositories.FilesRepository;
        }

        public async Task<string> AddImage(string previewFileId, string originalFileId)
        {
            var previewFileIdGuid = Guid.Parse(previewFileId);
            var originalFileIdGuid = Guid.Parse(originalFileId);

            var image = new Blog.Domain.Image();
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

        public async Task<byte[]> GetThumbnail(Guid imageFileId)
        {
            var file = await this.filesRepository.GetByIdAsync(imageFileId);

            using(var ms = new MemoryStream(file.Data))
            {
                var image = System.Drawing.Image.FromStream(ms);
                var thumbSize = this.GetThumbnailSize(image.Size);
                var thumbImage = image.GetThumbnailImage(thumbSize.Width, thumbSize.Height, null, IntPtr.Zero);

                ms.Seek(0, SeekOrigin.Begin);
                thumbImage.Save(ms, ImageFormat.Jpeg);
                await ms.FlushAsync();

                return ms.ToArray();
            }
        }

        private Size GetThumbnailSize(Size originalSize)
        {
            // Maximum size of any dimension.
            const int maxPixels = 100;

            // Compute best factor to scale entire image based on larger dimension.
            double factor;
            if (originalSize.Width > originalSize.Height)
            {
                factor = (double)maxPixels / originalSize.Width;
            }
            else
            {
                factor = (double)maxPixels / originalSize.Height;
            }

            // Return thumbnail size.
            return new Size((int)(originalSize.Width * factor), (int)(originalSize.Height * factor));
        }
    }
}
