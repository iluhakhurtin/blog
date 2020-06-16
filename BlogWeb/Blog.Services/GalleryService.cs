using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Blog.Services
{
    public interface IGalleryService
    {
        Task<string> AddGalleryItem(
            string smallFileId,
            string imageId,
            string articleId,
            string description);

        Task<string> EditGalleryItem(
            string id,
            string smallFileId,
            string imageId,
            string articleId,
            string description);
    }

    public class GalleryService : Service, IGalleryService
    {
        private readonly IGalleryRepository galleryRepository;

        public GalleryService(IRepositories repositories)
        {
            this.galleryRepository = repositories.GalleryRepository;
        }

        public async Task<string> AddGalleryItem(
            string smallFileId,
            string imageId,
            string articleId,
            string description)
        {
            var galleryItem = new GalleryItem();
            galleryItem.SmallPreviewFileId = Guid.Parse(smallFileId);
            galleryItem.ImageId = Guid.Parse(imageId);
            if (!string.IsNullOrEmpty(articleId))
                galleryItem.ArticleId = Guid.Parse(articleId);

            galleryItem.Description = description;

            await this.galleryRepository.AddAsync(galleryItem);

            return String.Empty;
        }

        public async Task<string> EditGalleryItem(
            string id,
            string smallFileId,
            string imageId,
            string articleId,
            string description)
        {
            var galleryItemId = Guid.Parse(id);

            var galleryItem = await this.galleryRepository.GetAsync(galleryItemId);
            galleryItem.SmallPreviewFileId = Guid.Parse(smallFileId);
            galleryItem.ImageId = Guid.Parse(imageId);

            if (!string.IsNullOrEmpty(articleId))
                galleryItem.ArticleId = Guid.Parse(articleId);

            galleryItem.Description = description;

            await this.galleryRepository.UpdateAsync(galleryItem);

            return String.Empty;
        }
    }
}
