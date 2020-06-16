using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Retrievers.File;
using Blog.Retrievers.Gallery;
using Blog.Retrievers.Image;
using Blog.Services;
using Blog.Web.Models.jqGrid;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Controllers.Administration
{
    [Route("api/Administration/GalleryApi")]
    public class GalleryApiController : BaseApiAdministrationController
    {
        private readonly IGalleryRetriever galleryRetriever;
        private readonly IGalleryRepository galleryRepository;
        private readonly IGalleryService galleryService;

        public GalleryApiController(
            ILog log,
            IServices services,
            IRetrievers retrievers,
            IRepositories repositories)
            : base(log)
        {
            this.galleryService = services.GalleryService;
            this.galleryRetriever = retrievers.GalleryRetriever;
            this.galleryRepository = repositories.GalleryRepository;
        }

        // GET: api/GalleryApi
        [HttpGet]
        public async Task<jqGridResult> Get(
            bool _search,
            long nd,
            int page,
            int rows,
            string sidx,
            string sord,
            int id,
            string filters)
        {
            try
            {
                string smallFileName = null;
                string previewFileName = null;
                string originalFileName = null;
                string articleTitle = null;
                string description = null;


                if (!String.IsNullOrEmpty(filters))
                {
                    jqGridFilter filter = JsonConvert.DeserializeObject<jqGridFilter>(filters);
                    smallFileName = filter.GetFilterByFieldName(GalleryPagedDataTable.SmallFileName);
                    previewFileName = filter.GetFilterByFieldName(GalleryPagedDataTable.PreviewFileName);
                    originalFileName = filter.GetFilterByFieldName(GalleryPagedDataTable.OriginalFileName);
                    articleTitle = filter.GetFilterByFieldName(GalleryPagedDataTable.ArticleTitle);
                    description = filter.GetFilterByFieldName(GalleryPagedDataTable.Description);
                }

                var pagedDataTable = await this.galleryRetriever.GetGalleryPagedAsync(
                    smallFileName,
                    previewFileName,
                    originalFileName,
                    articleTitle,
                    description,
                    sidx,
                    sord,
                    page,
                    rows);

                var result = new jqGridResult(
                    pagedDataTable.TotalPagesCount,
                    pagedDataTable.PageNumber,
                    pagedDataTable.Rows.Count,
                    pagedDataTable,
                    PagedDataTable.Id);

                return result;
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return null;
            }
        }

        // POST: api/GalleryApi
        [HttpPost]
        public async Task<HttpResponseMessage> Post(
            [FromForm]string id,
            [FromForm]string oper,
            [FromForm] string smallFileId,
            [FromForm]string imageId,
            [FromForm]string articleId,
            [FromForm]string description)
        {

            try
            {
                switch (oper)
                {
                    case jqGridActions.Add:
                        return await this.AddGalleryItem(smallFileId, imageId, articleId, description);

                    case jqGridActions.Edit:
                        return await this.EditGalleryItem(id, smallFileId, imageId, articleId, description);

                    case jqGridActions.Delete:
                        return await this.DeleteGalleryItem(id);
                }
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                return base.CreateErrorResponseMessage();
            }
            return base.CreateErrorResponseMessage("Not implemented");
        }

        private async Task<HttpResponseMessage> AddGalleryItem(
            string smallFileId,
            string imageId,
            string articleId,
            string description)
        {
            var error = await this.galleryService.AddGalleryItem(smallFileId, imageId, articleId, description);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> EditGalleryItem(
            string id,
            string smallFileId,
            string imageId,
            string articleId,
            string description)
        {
            var error = await this.galleryService.EditGalleryItem(id, smallFileId, imageId, articleId, description);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> DeleteGalleryItem(string id)
        {
            var galleryItemId = Guid.Parse(id);
            await this.galleryRepository.DeleteAsync(galleryItemId);

            var httpResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(httpResponseMessage);
        }
    }
}
