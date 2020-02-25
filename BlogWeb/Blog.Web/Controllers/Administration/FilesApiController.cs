using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Retrievers.File;
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
    [Route("api/Administration/FilesApi")]
    public class FilesApiController : BaseApiAdministrationController
    {
        private readonly IFilesRetriever filesRetriever;
        private readonly IFilesService filesService;
        private readonly IFilesRepository filesRepository;

        public FilesApiController(
            ILog log,
            IServices services,
            IRetrievers retrievers,
            IRepositories repositories)
            : base(log)
        {
            this.filesRetriever = retrievers.FilesRetriever;
            this.filesService = services.FilesService;
            this.filesRepository = repositories.FilesRepository;
        }

        // GET: api/FilesApi
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
                string name = null;
                string extension = null;
                string mimeType = null;

                if (!String.IsNullOrEmpty(filters))
                {
                    jqGridFilter filter = JsonConvert.DeserializeObject<jqGridFilter>(filters);
                    name = filter.GetFilterByFieldName(FilesPagedDataTable.Name);
                    extension = filter.GetFilterByFieldName(FilesPagedDataTable.Extension);
                    mimeType = filter.GetFilterByFieldName(FilesPagedDataTable.MimeType);
                }

                var pagedDataTable = await this.filesRetriever.GetFilesPagedAsync(
                    name,
                    extension,
                    mimeType,
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

        // POST: api/FilesApi
        [HttpPost]
        public async Task<HttpResponseMessage> Post(
            [FromForm]string id,
            [FromForm]string oper,
            [FromForm] string Name,
            [FromForm] string Extension,
            [FromForm] string MimeType,
            [FromForm] IFormFile File)
        {

            try
            {
                switch (oper)
                {
                    case jqGridActions.Add:
                        return await this.AddFile(Name, Extension, MimeType, File);

                    case jqGridActions.Edit:
                        return await this.EditFile(id, Name, Extension, MimeType);

                    case jqGridActions.Delete:
                        return await this.DeleteFile(id);
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

        private async Task<HttpResponseMessage> AddFile(string name, string extension, string mimeType, IFormFile file)
        {
            var error = await this.filesService.AddFile(name, extension, mimeType, file);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> EditFile(
            string id,
            string name,
            string extension,
            string mimeType)
        {
            var error = await this.filesService.EditFile(id, name, extension, mimeType);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> DeleteFile(string id)
        {
            var fileId = Guid.Parse(id);
            await this.filesRepository.DeleteAsync(fileId);

            var httpResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(httpResponseMessage);
        }
    }
}
