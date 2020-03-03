using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Web.Models.Articles;
using log4net;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class SearchArticlesController : BaseController
    {
        private readonly IArticlesRetriever articlesRetriever;
        private readonly ICategoriesRepository categoriesRepository;

        public SearchArticlesController(ILog log, IRetrievers retrievers, IRepositories repositories)
            : base(log)
        {
            this.articlesRetriever = retrievers.ArticlesRetriever;
            this.categoriesRepository = repositories.CategoriesRepository;
        }

        public async Task<IActionResult> Search(SearchArticlesViewModel viewModel)
        {
            try
            {
                if (viewModel == null)
                {
                    viewModel = new SearchArticlesViewModel();
                }

                if (!String.IsNullOrEmpty(viewModel.SearchPattern))
                {
                    var articles = await this.articlesRetriever.FindArticlesAsync(viewModel.SearchPattern);
                    viewModel.Articles.AddRange(articles);
                }

                return View("Index", viewModel);
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        public async Task<IActionResult> SearchByCategory(Guid id)
        {
            try
            {
                var category = await this.categoriesRepository.GetAsync(id);
                var articles = await this.articlesRetriever.GetCategoryArticlesAsync(id);
                var viewModel = new SearchArticlesViewModel(articles);
                viewModel.SearchPattern = category.Name;

                return View("Index", viewModel);
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
