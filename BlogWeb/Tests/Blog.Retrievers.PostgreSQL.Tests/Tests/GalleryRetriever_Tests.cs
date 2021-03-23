using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers.Gallery;
using Xunit;

namespace Blog.Retrievers.PostgreSQL.Tests.Tests
{
    public class GalleryRetriever_Tests : BaseRetrieverTests
    {
        private readonly IGalleryRetriever galleryRetriever;

        private readonly IGalleryRepository galleryRepository;
        private readonly IImagesRepository imagesRepository;
        private readonly IFilesRepository filesRepository;

        public GalleryRetriever_Tests()
        {
            IRetrievers retrievers = DependencyResolver.Resolve<IRetrievers>();
            this.galleryRetriever = retrievers.GalleryRetriever;

            IRepositories repositories = DependencyResolver.Resolve<IRepositories>();
            this.galleryRepository = repositories.GalleryRepository;
            this.imagesRepository = repositories.ImagesRepository;
            this.filesRepository = repositories.FilesRepository;
        }

        [Fact]
        public async Task Can_GetGalleryItemsPagedAsync()
        {
            var galleryItem1 = await this.BuildGalleryItemAsync(1);
            var galleryItem2 = await this.BuildGalleryItemAsync(2);
            var galleryItem3 = await this.BuildGalleryItemAsync(3);
            var galleryItem4 = await this.BuildGalleryItemAsync(4);
            var galleryItem5 = await this.BuildGalleryItemAsync(5);
            var galleryItem6 = await this.BuildGalleryItemAsync(6);

            var pageNumber = 3;
            var pageSize = 2;

            var actual = await this.galleryRetriever.GetGalleryItemsPagedAsync(pageNumber, pageSize);

            Assert.Equal(3, actual.TotalPagesCount);
            Assert.Equal(6, actual.TotalResultsCount);
            Assert.Equal(pageSize, actual.PageSize);
            Assert.Equal(pageNumber, actual.PageNumber);

            Assert.Equal(2, actual.Items.Count);
            Assert.Equal(galleryItem5.Id, actual.Items[0].Id);
            Assert.Equal(galleryItem6.Id, actual.Items[1].Id);
        }

        private async Task<Domain.File> BuildFileAsync(string fileName)
        {
            var file = new Domain.File();
            file.Id = Guid.NewGuid();
            file.Name = fileName;
            file.Extension = ".jpg";
            file.MimeType = "image/jpeg";
            file.Data = new byte[1];

            await this.filesRepository.AddAsync(file);

            return file;
        }

        private async Task<Domain.Image> BuildImageAsync(Guid originalFileId, Guid previewFileId)
        {
            var image = new Domain.Image();
            image.Id = Guid.NewGuid();
            image.OriginalFileId = originalFileId;
            image.PreviewFileId = previewFileId;

            await this.imagesRepository.AddAsync(image);

            return image;
        }

        private async Task<GalleryItem> BuildGalleryItemAsync(int number)
        {
            var numberStr = number.ToString();

            var originalFile = await this.BuildFileAsync("Original file " + numberStr);
            var previewFile = await this.BuildFileAsync("Preview file " + numberStr);
            var image = await this.BuildImageAsync(originalFile.Id, previewFile.Id);
            var smallPreviewFile = await this.BuildFileAsync("Small preview file " + numberStr);
            
            var galleryItem = new GalleryItem();
            galleryItem.Id = Guid.NewGuid();
            galleryItem.ImageId = image.Id;
            galleryItem.SmallPreviewFileId = smallPreviewFile.Id;
            galleryItem.Title = "Title of the gallery item " + numberStr;
            galleryItem.Description = "Description of the gallery item " + numberStr;

            await this.galleryRepository.AddAsync(galleryItem);

            return galleryItem;
        }
    }
}
