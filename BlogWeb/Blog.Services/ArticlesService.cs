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
        Task EditArticle(Guid id, string title, string body, IEnumerable<string> roles, IEnumerable<Guid> categoryIds);
    }


    public class ArticlesService: Service, IArticlesService
    {
        private readonly IArticlesRepository articlesRepository;
        private readonly IArticleRolesRepository articleRolesRepository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IArticleCategoriesRepository articleCategoriesRepository;

        public ArticlesService(IRepositories repositories, RoleManager<ApplicationRole> roleManager)
        {
            this.articlesRepository = repositories.ArticlesRepository;
            this.articleRolesRepository = repositories.ArticleRolesRepository;
            this.articleCategoriesRepository = repositories.ArticleCategoriesRepository;
            this.roleManager = roleManager;
        }

        public async Task AddArticle(
            string title,
            string body,
            IEnumerable<string> roles,
            IEnumerable<Guid> categoryIds)
        {
            var article = new Article();
            article.Title = title;
            article.Body = body;

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

        public async Task EditArticle(
            Guid id,
            string title,
            string body,
            IEnumerable<string> roles,
            IEnumerable<Guid> categoryIds)
        {
            var article = await this.articlesRepository.GetAsync(id);

            if (article.Title != title || article.Body != body)
            {
                article.Title = title;
                article.Body = body;
                await this.articlesRepository.UpdateAsync(article);
            }

            await this.articleRolesRepository.DeleteAllForArticleAsync(id);
            if(roles != null)
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

            await this.articleCategoriesRepository.DeleteAllForArticleAsync(id);
            if(categoryIds != null)
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
