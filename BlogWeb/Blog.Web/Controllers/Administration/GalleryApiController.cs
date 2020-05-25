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

        public GalleryApiController(
            ILog log,
            IServices services,
            IRetrievers retrievers,
            IRepositories repositories)
            : base(log)
        {
            this.galleryRetriever = retrievers.GalleryRetriever;
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
            [FromForm]string previewFileId,
            [FromForm]string originalFileId)
        {

            try
            {
                switch (oper)
                {
                    case jqGridActions.Add:
                        return await this.AddImage(previewFileId, originalFileId);

                    case jqGridActions.Edit:
                        return await this.EditImage(id, previewFileId, originalFileId);

                    case jqGridActions.Delete:
                        return await this.DeleteImage(id);
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

        private async Task<HttpResponseMessage> AddImage(string previewFileId, string originalFileId)
        {
            throw new NotImplementedException();
            //var error = await this.imagesService.AddImage(previewFileId, originalFileId);

            //if (!String.IsNullOrEmpty(error))
            //{
            //    var errorResponseMessage = base.CreateErrorResponseMessage(error);
            //    return await Task.FromResult(errorResponseMessage);
            //}

            //var okResponseMessage = base.CreateOkResponseMessage();
            //return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> EditImage(
            string id,
            string previewFileId,
            string originalFileId)
        {
            throw new NotImplementedException();
            //var error = await this.imagesService.EditImage(id, previewFileId, originalFileId);

            //if (!String.IsNullOrEmpty(error))
            //{
            //    var errorResponseMessage = base.CreateErrorResponseMessage(error);
            //    return await Task.FromResult(errorResponseMessage);
            //}

            //var okResponseMessage = base.CreateOkResponseMessage();
            //return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> DeleteImage(string id)
        {
            throw new NotImplementedException();
            //var fileId = Guid.Parse(id);
            //await this.imagesRepository.DeleteAsync(fileId);

            //var httpResponseMessage = base.CreateOkResponseMessage();
            //return await Task.FromResult(httpResponseMessage);
        }
    }
}
