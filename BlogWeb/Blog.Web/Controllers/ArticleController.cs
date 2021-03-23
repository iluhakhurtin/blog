using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Retrievers.Image;
using Blog.Web.Models.Article;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Blog.Web.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticlesRetriever articlesRetriever;
        private readonly IImagesRetriever imagesRetriever;

        public ArticleController(ILog log, IRetrievers retrievers)
            : base(log)
        {
            this.articlesRetriever = retrievers.ArticlesRetriever;
            this.imagesRetriever = retrievers.ImagesRetriever;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index(Guid id)
        {
            try
            {
                var articleWithRolesDataResult = await this.articlesRetriever.GetArticleWithRolesAsync(id);
                var articleRoles = articleWithRolesDataResult.GetRoles();
                if(articleRoles != null && !User.IsInRole(ApplicationRole.Administrator))
                {
                    var authorized = false;
                    foreach (var role in articleRoles)
                    {
                        if (User.IsInRole(role))
                        {
                            authorized = true;
                            break;
                        }
                    }

                    if (!authorized)
                        return base.Unauthorized();
                }

                var articleViewModel = new ArticleViewModel(articleWithRolesDataResult);
                return View(articleViewModel);
            }
            catch(Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        // GET: /<controller>/images
        public async Task<IActionResult> Image(string fileName)
        {
            try
            {
                if (fileName.Contains("prev"))
                {
                    return await this.GetPreviewImage(fileName);
                }
                else
                {
                    return await this.GetOriginalImage(fileName);
                }
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        private async Task<IActionResult> GetPreviewImage(string fileName)
        {
            var imageDataResult = await this.imagesRetriever.GetPreviewImageDataByNameAsync(fileName);
            if (imageDataResult == null)
                return base.NotFound();
            
            var fileContentResult = new FileContentResult(imageDataResult.Data, imageDataResult.MimeType);
            return fileContentResult;
        }

        private async Task<IActionResult> GetOriginalImage(string fileName)
        {
            if (User.IsInRole(ApplicationRole.Administrator) || User.IsInRole(ApplicationRole.ImageViewer))
            {
                var imageDataResult = await this.imagesRetriever.GetOriginalImageDataByNameAsync(fileName);
                if (imageDataResult == null)
                    return base.NotFound();

                var fileContentResult = new FileContentResult(imageDataResult.Data, imageDataResult.MimeType);
                return fileContentResult;
            }
            return base.Unauthorized();
        }
    }
}
