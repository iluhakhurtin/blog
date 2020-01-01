using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Retrievers;
using Blog.Web.Models.Articles;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ArticlesController : BaseController
    {
        private readonly IArticlesRetriever articlesRetriever;


        public ArticlesController(IRetrievers retrievers)
        {
            this.articlesRetriever = retrievers.ArticlesRetriever;
        }

        public async Task<IActionResult> Index(Guid id)
        {
            var articles = await this.articlesRetriever.GetCategoryArticlesAsync(id);
            var articlesViewModel = new ArticlesViewModel(articles);

            return View(articlesViewModel);
        }
    }
}
