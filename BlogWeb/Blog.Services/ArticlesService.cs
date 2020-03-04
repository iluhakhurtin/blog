using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services
{
    public interface IArticlesService
    {
        Task<string> AddArticle(string coverFileId, string title, string body, string csvRoles, string csvCategories);
        Task<string> EditArticle(string id, string coverFileId, string title, string body, string csvRoles, string csvCategories);
    }


    public class ArticlesService: Service, IArticlesService
    {
        private readonly IArticlesRepository articlesRepository;
        private readonly IArticleRolesRepository articleRolesRepository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IArticleCategoriesRepository articleCategoriesRepository;
        private readonly IStringService stringService;
        private readonly IRolesService rolesService;

        public ArticlesService(IRepositories repositories,
            IStringService stringService,
            IRolesService rolesService,
            RoleManager<ApplicationRole> roleManager)
        {
            this.articlesRepository = repositories.ArticlesRepository;
            this.articleRolesRepository = repositories.ArticleRolesRepository;
            this.articleCategoriesRepository = repositories.ArticleCategoriesRepository;
            this.roleManager = roleManager;
            this.stringService = stringService;
            this.rolesService = rolesService;
        }

        public async Task<string> AddArticle(
            string coverFileId,
            string title,
            string body,
            string csvRoles,
            string csvCategories)
        {
            //validate the roles
            var roles = this.stringService.ParseCsvStringToArray(csvRoles);
            if (roles != null)
            {
                string errorMessage = await this.rolesService.ValidateRoles(roles);
                if (!String.IsNullOrEmpty(errorMessage))
                    return errorMessage;
            }

            if (String.IsNullOrEmpty(title))
            {
                return "Title cannot be empty.";
            }

            if (String.IsNullOrEmpty(body))
            {
                return "Body cannot be empty.";
            }

            var article = new Article();
            article.Title = title;
            article.Body = body;

            if (!String.IsNullOrEmpty(coverFileId))
                article.CoverFileId = Guid.Parse(coverFileId);

            await this.articlesRepository.AddAsync(article);

            await this.UpdateArticleRoles(article, roles);
            await this.UpdateArticleCategories(article, csvCategories);

            return String.Empty;
        }

        public async Task<string> EditArticle(
            string id,
            string coverFileId,
            string title,
            string body,
            string csvRoles,
            string csvCategories)
        {
            var articleId = Guid.Parse(id);

            //validate the roles
            var roles = this.stringService.ParseCsvStringToArray(csvRoles);
            if (roles != null)
            {
                string errorMessage = await this.rolesService.ValidateRoles(roles);
                if(!String.IsNullOrEmpty(errorMessage))
                    return errorMessage;
            }

            var article = await this.articlesRepository.GetAsync(articleId);

            if (!String.IsNullOrEmpty(title) && article.Title != title)
            {
                article.Title = title;
            }

            if (!String.IsNullOrEmpty(body) && article.Body != body)
            {
                article.Body = body;
            }

            Guid? newCoverFileId = null;
            if (!String.IsNullOrEmpty(coverFileId))
                newCoverFileId = Guid.Parse(coverFileId);

            if (article.CoverFileId != newCoverFileId)
            {
                article.CoverFileId = newCoverFileId;
            }

            await this.articlesRepository.UpdateAsync(article);
            await this.UpdateArticleRoles(article, roles);
            await this.UpdateArticleCategories(article, csvCategories);

            return String.Empty;
        }

        private async Task UpdateArticleRoles(Article article, string[] roles)
        {
            await this.articleRolesRepository.DeleteAllForArticleAsync(article.Id);

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    var applicationRole = await this.roleManager.FindByNameAsync(role);
                    var articleRole = new ArticleRole();
                    articleRole.Article = article;
                    articleRole.Role = applicationRole;

                    await this.articleRolesRepository.AddAsync(articleRole);
                }
            }
        }

        private async Task UpdateArticleCategories(Article article, string csvCategories)
        {
            var categories = this.stringService.ParseCsvStringToArray(csvCategories);
            List<Guid> categoryIds = null;
            if (categories != null)
            {
                categoryIds = new List<Guid>();
                foreach (var category in categories)
                {
                    var categoryId = Guid.Parse(category);
                    categoryIds.Add(categoryId);
                }
            }

            await this.articleCategoriesRepository.DeleteAllForArticleAsync(article.Id);

            if (categoryIds != null)
            {
                foreach (var categoryId in categoryIds)
                {
                    var articleCategory = new ArticleCategory();
                    articleCategory.Article = article;
                    articleCategory.CategoryId = categoryId;

                    await this.articleCategoriesRepository.AddAsync(articleCategory);
                }
            }
        }
    }
}
