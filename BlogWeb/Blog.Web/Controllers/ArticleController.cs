using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Web.Models.Article;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticlesRepository articlesRepository;

        public ArticleController(IRepositories repositories)
        {
            this.articlesRepository = repositories.ArticlesRepository;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index(Guid id)
        {
            var article = await this.articlesRepository.GetAsync(id);
            var articleViewModel = new ArticleViewModel(article);

            return View(articleViewModel);
        }

        // GET: /<controller>/images
        public IActionResult Image(string fileName)
        {
            //try
            //{
            //    var imageDataResult = await this.imagesRetriever.GetOriginalImageDataAsync(id);
            //    var fileContentResult = new FileContentResult(imageDataResult.Data, imageDataResult.MimeType);
            //    return fileContentResult;
            //}
            //catch (Exception ex)
            //{
            //    return base.NotFound();
            //}
            return base.NotFound();
        }
    }
}
