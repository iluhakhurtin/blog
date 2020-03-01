using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Image;
using Blog.Services;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class FileController : BaseController
    {
        private readonly IFilesRepository filesRepository;
        private readonly IImagesService imagesService;

        public FileController(ILog log, IRepositories repositories, IServices services)
            : base(log)
        {
            this.filesRepository = repositories.FilesRepository;
            this.imagesService = services.ImagesService;
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
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        public async Task<IActionResult> Thumbnail(Guid id)
        {
            try
            {
                var thumbBytes = await this.imagesService.GetThumbnail(id);
                var fileContentResult = new FileContentResult(thumbBytes, "image/jpeg");
                return fileContentResult;
            }
            catch(Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }
    }
}
