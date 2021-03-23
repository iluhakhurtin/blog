using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blog.Web.Models;
using log4net;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Web.Models.Home;
using Microsoft.AspNetCore.Identity;
using Blog.Domain;

namespace Blog.Web.Controllers
{
    public class HomeController : BaseUserManagerController
    {
        private readonly IArticlesRetriever articlesRetriever;


        public HomeController(ILog log, IRetrievers retrievers, UserManager<ApplicationUser> userManager)
            : base(log, userManager)
        {
            this.articlesRetriever = retrievers.ArticlesRetriever;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 9)
        {
            try
            {
                var userRoles = await base.GetUserRoles();
                var articleItems = await this.articlesRetriever.GetLatestArticleItemsPagedAsync(pageNumber, pageSize, userRoles);
                var viewModel = new HomeViewModel(articleItems);
                return View(viewModel);
            }
            catch(Exception ex)
            {
                if(base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
