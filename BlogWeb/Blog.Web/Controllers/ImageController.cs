﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Image;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImagesRetriever imagesRetriever;
        private readonly IFilesRepository filesRepository;

        public ImageController(ILog log, IRepositories repositories, IRetrievers retrievers)
            : base(log)
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
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        [Authorize(Roles = ApplicationRole.Administrator + "," + ApplicationRole.ImageViewer)]
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
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }
    }
}
