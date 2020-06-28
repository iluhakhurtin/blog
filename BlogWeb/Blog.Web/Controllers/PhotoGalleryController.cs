using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Retrievers.Gallery;
using Blog.Retrievers.Image;
using Blog.Web.Models.Article;
using Blog.Web.Models.Gallery;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class PhotoGalleryController : BaseController
    {
        private readonly IGalleryRetriever galleryRetriever;
        private readonly IGalleryRepository galleryRepository;

        public PhotoGalleryController(ILog log, IRetrievers retrievers, IRepositories repositories)
            : base(log)
        {
            this.galleryRetriever = retrievers.GalleryRetriever;
            this.galleryRepository = repositories.GalleryRepository;
        }


        // GET: /<controller>/
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var galleryItems = await this.galleryRetriever.GetGalleryItemsPagedAsync(pageNumber, pageSize);
                var viewModel = new PhotoGalleryViewModel(galleryItems);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.NotFound();
            }
        }

        public async Task<IActionResult> Preview(Guid id)
        {
            try
            {
                var galleryItem = await this.galleryRepository.GetAsync(id);
                var viewModel = new PhotoGalleryPreviewViewModel(galleryItem);
                return View(viewModel);
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
