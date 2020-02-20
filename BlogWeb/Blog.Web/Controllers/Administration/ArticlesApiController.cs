using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Blog.Retrievers.Article;
using Blog.Services;
using Blog.Web.Models.jqGrid;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Web.Controllers.Administration
{
    [Route("api/Administration/ArticlesApi")]
    public class ArticlesApiController : BaseApiAdministrationController
    {
        private readonly IArticlesRepository articlesRepository;
        private readonly IArticlesRetriever articlesRetriever;
        private readonly IArticlesService articlesService;

        public ArticlesApiController(
            ILog log,
            IServices services,
            IRetrievers retrievers,
            IRepositories repositories)
            : base(log)
        {
            this.articlesRepository = repositories.ArticlesRepository;
            this.articlesRetriever = retrievers.ArticlesRetriever;
            this.articlesService = services.ArticlesService;
        }

        // GET: api/ArticlesApi/GetArticleBody
        [HttpGet("GetArticleBody")]
        public async Task<String> GetArticleBody(Guid id)
        {
            try
            {
                var article = await this.articlesRepository.GetAsync(id);
                return article.Body;
            }
            catch(Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);
            }
            return String.Empty;
        }

        // GET: api/ArticlesApi
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
                string title = null;
                string roles = null;
                string categories = null;

                if (!String.IsNullOrEmpty(filters))
                {
                    jqGridFilter filter = JsonConvert.DeserializeObject<jqGridFilter>(filters);
                    title = filter.GetFilterByFieldName(ArticlesPagedDataTable.Title);
                    roles = filter.GetFilterByFieldName(ArticlesPagedDataTable.Roles);
                    categories = filter.GetFilterByFieldName(ArticlesPagedDataTable.Categories);
                }

                var pagedDataTable = await this.articlesRetriever.GetArticlesPagedAsync(
                    title,
                    roles,
                    categories,
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

        // POST: api/ArticlesApi
        [HttpPost]
        public async Task<HttpResponseMessage> Post(
            [FromForm]string id,
            [FromForm]string oper,
            [FromForm]string Title,
            [FromForm]string Body,
            [FromForm]string Roles,
            [FromForm]string Categories)
        {

            try
            {
                switch (oper)
                {
                    case jqGridActions.Add:
                        return await this.AddArticle(Title, Body, Roles, Categories);

                    case jqGridActions.Edit:
                        return await this.EditArticle(id, Title, Body, Roles, Categories);

                    case jqGridActions.Delete:
                        return await this.DeleteArticle(id);
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

        private async Task<HttpResponseMessage> AddArticle(string title, string body, string roles, string categories)
        {
            var error = await this.articlesService.AddArticle(title, body, roles, categories);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> EditArticle(
            string id,
            string title,
            string body,
            string roles,
            string categories)
        {
            var error = await this.articlesService.EditArticle(id, title, body, roles, categories);

            if (!String.IsNullOrEmpty(error))
            {
                var errorResponseMessage = base.CreateErrorResponseMessage(error);
                return await Task.FromResult(errorResponseMessage);
            }

            var okResponseMessage = base.CreateOkResponseMessage();
            return await Task.FromResult(okResponseMessage);
        }

        private async Task<HttpResponseMessage> DeleteArticle(string id)
        {
            var articleId = Guid.Parse(id);
            HttpResponseMessage httpResponseMessage = base.CreateOkResponseMessage();
            try
            {
                await this.articlesRepository.DeleteAsync(articleId);
            }
            catch (Exception ex)
            {
                if (base.log.IsErrorEnabled)
                    base.log.Error(ex);

                httpResponseMessage = base.CreateErrorResponseMessage(ex.Message);
            }
            return await Task.FromResult(httpResponseMessage);
        }
    }
}
