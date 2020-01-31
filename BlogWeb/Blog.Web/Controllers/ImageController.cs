using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Image;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImagesRetriever imagesRetriever;
        private readonly IFilesRepository filesRepository;

        public ImageController(IRepositories repositories, IRetrievers retrievers)
        {
            this.filesRepository = repositories.FilesRepository;
            this.imagesRetriever = retrievers.ImagesRetriever;
        }


        public async Task<IActionResult> Preview(Guid id)
        {
            try
            {
                var image = await this.imagesRetriever.GetPreviewImageDataAsync(id);
                var fileContentResult = new FileContentResult(image.Data, image.MimeType);
                return fileContentResult;
            }
            catch (Exception ex)
            {
                return base.NotFound();
            }
        }

        public async Task<IActionResult> Original(Guid id)
        {
            try
            {
                var image = await this.imagesRetriever.GetOriginalImageDataAsync(id);
                var fileContentResult = new FileContentResult(image.Data, image.MimeType);
                return fileContentResult;
            }
            catch (Exception ex)
            {
                return base.NotFound();
            }
        }

        public async Task<IActionResult> Index(Guid id)
        {
            try
            {
                var file = await this.filesRepository.GetByIdAsync(id);
                var fileContentResult = new FileContentResult(file.Data, file.MimeType);
                return fileContentResult;
            }
            catch(Exception ex)
            {
                return base.NotFound();
            }
        }
    }
}
