using System;
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
        private readonly IFilesRepository filesRepository;

        public ArticleController(IRepositories repositories)
        {
            this.articlesRepository = repositories.ArticlesRepository;
            this.filesRepository = repositories.FilesRepository;
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
                var file = await this.filesRepository.GetByNameAsync(fileName);
                var fileContentResult = new FileContentResult(file.Data, file.MimeType);
                return fileContentResult;
            }
            catch (Exception ex)
            {
                return base.NotFound();
            }
        }
    }
}
