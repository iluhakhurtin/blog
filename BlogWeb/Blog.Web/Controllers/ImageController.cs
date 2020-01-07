using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repositories;
using Blog.Retrievers;
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

        [HttpGet]
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
