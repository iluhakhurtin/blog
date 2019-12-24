﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Web.Models.Article;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticlesRepository articlesRepository;
        private readonly IImagesRetriever imagesRetriever;

        public ArticleController(IRepositories repositories, IRetrievers retrievers)
        {
            this.articlesRepository = repositories.ArticlesRepository;
            this.imagesRetriever = retrievers.ImagesRetriever;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index(Guid id)
        {
            var article = await this.articlesRepository.GetAsync(id);
            var articleViewModel = new ArticleViewModel(article);

            return View(articleViewModel);
        }

        // GET: /<controller>/images
        public async Task<IActionResult> Image(string fileName)
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
