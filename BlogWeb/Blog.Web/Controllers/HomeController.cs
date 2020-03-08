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

        public async Task<IActionResult> Index(int count)
        {
            try
            {
                if (count == 0)
                    count = 24;

                var userRoles = await base.GetUserRoles();
                var articles = await this.articlesRetriever.GetLatestArticles(count, userRoles);
                var viewModel = new HomeViewModel(articles);
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
