using System;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers.Article;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace Blog.Retrievers.PostgreSQL.Tests.Article
{
    public class ArticlesRetriever_Tests : BaseRetrieverTests
    {
        private readonly IArticlesRetriever articlesRetriever;
        private readonly IArticlesRepository articlesRepository;
        private readonly IArticleRolesRepository articleRolesRepository;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ArticlesRetriever_Tests()
        {
            IRetrievers retrievers = DependencyResolver.Resolve<IRetrievers>();
            this.articlesRetriever = retrievers.ArticlesRetriever;

            IRepositories repositories = DependencyResolver.Resolve<IRepositories>();
            this.articlesRepository = repositories.ArticlesRepository;
            this.articleRolesRepository = repositories.ArticleRolesRepository;

            this.roleManager = DependencyResolver.Resolve<RoleManager<ApplicationRole>>();
        }

        [Fact]
        public async Task Can_GetGetArticleWithRolesAsync()
        {
            var expectedId = Guid.NewGuid();
            var expectedTitle = "some title";
            var expectedBody = "some body";
            var expectedTimestamp = DateTime.ParseExact("01.02.2020", "dd.MM.yyyy", null);

            var article = new Blog.Domain.Article();
            article.Id = expectedId;
            article.Title = expectedTitle;
            article.Body = expectedBody;
            article.Timestamp = expectedTimestamp;

            await this.articlesRepository.AddAsync(article);

            var role1 = new ApplicationRole("role1");
            var identityResult = await this.roleManager.CreateAsync(role1);
            Assert.True(identityResult.Succeeded);

            var role2 = new ApplicationRole("role2");
            identityResult = await this.roleManager.CreateAsync(role2);
            Assert.True(identityResult.Succeeded);

            var articleRole1 = new ArticleRole();
            articleRole1.Article = article;
            articleRole1.Role = role1;

            await this.articleRolesRepository.AddAsync(articleRole1);

            var articleRole2 = new ArticleRole();
            articleRole2.Article = article;
            articleRole2.Role = role2;

            await this.articleRolesRepository.AddAsync(articleRole2);

            var actual = await this.articlesRetriever.GetArticleWithRolesAsync(article.Id);

            Assert.Equal(expectedId, actual.Id);
            Assert.Equal(expectedTitle, actual.Title);
            Assert.Equal(expectedBody, actual.Body);
            Assert.Equal(expectedTimestamp, actual.Timestamp);
            Assert.True("role1, role2" == actual.Roles || "role2, role1" == actual.Roles);
        }

        [Fact]
        public async Task Can_GetCategoryArticlesAsync()
        {
            
        }
    }
}
