﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blog.Retrievers;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IImagesRetriever imagesRetriever;

        public ImageController(IRetrievers retrievers)
        {
            this.imagesRetriever = retrievers.ImagesRetriever;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid id)
        {
            try
            {
                var imageDataResult = await this.imagesRetriever.GetOriginalImageDataAsync(id);
                var fileContentResult = new FileContentResult(imageDataResult.Data, imageDataResult.MimeType);
                return fileContentResult;
            }
            catch(Exception ex)
            {
                return base.NotFound();
            }
        }

        public async Task<IActionResult> Original(string fileName)
        {
            try
            {
                var imageDataResult = await this.imagesRetriever.GetPreviewImageDataByNameAsync(fileName);
                var fileContentResult = new FileContentResult(imageDataResult.Data, imageDataResult.MimeType);
                return fileContentResult;
            }
            catch (Exception ex)
            {
                return base.NotFound();
            }
        }
    }
}
