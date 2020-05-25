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
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IArticleCategoriesRepository articleCategoriesRepository;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ArticlesRetriever_Tests()
        {
            IRetrievers retrievers = DependencyResolver.Resolve<IRetrievers>();
            this.articlesRetriever = retrievers.ArticlesRetriever;

            IRepositories repositories = DependencyResolver.Resolve<IRepositories>();
            this.articlesRepository = repositories.ArticlesRepository;
            this.articleRolesRepository = repositories.ArticleRolesRepository;
            this.categoriesRepository = repositories.CategoriesRepository;
            this.articleCategoriesRepository = repositories.ArticleCategoriesRepository;

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
            var category1 = new Category();
            category1.Name = "category 1";

            await this.categoriesRepository.AddAsync(category1);

            var category2 = new Category();
            category2.Name = "category 2";
            category2.Parent = category1;

            await this.categoriesRepository.AddAsync(category2);

            var category3 = new Category();
            category3.Name = "category 3";
            category3.Parent = category2;

            await this.categoriesRepository.AddAsync(category3);

            var category4 = new Category();
            category4.Name = "category 4";
            category4.Parent = category3;

            await this.categoriesRepository.AddAsync(category4);

            var category5 = new Category();
            category5.Name = "category 5";
            category5.Parent = category2;

            await this.categoriesRepository.AddAsync(category5);

            var category6 = new Category();
            category6.Name = "category 6";
            category6.Parent = category5;

            await this.categoriesRepository.AddAsync(category6);

            var article1 = new Blog.Domain.Article();
            article1.Title = "title article 1";
            article1.Body = "body article 1";

            await this.articlesRepository.AddAsync(article1);

            var article1Category2 = new ArticleCategory();
            article1Category2.Article = article1;
            article1Category2.Category = category2;

            await this.articleCategoriesRepository.AddAsync(article1Category2);

            var article1Category5 = new ArticleCategory();
            article1Category5.Article = article1;
            article1Category5.Category = category5;

            await this.articleCategoriesRepository.AddAsync(article1Category5);

            var article2 = new Blog.Domain.Article();
            article2.Title = "title article 2";
            article2.Body = "body article 2";

            await this.articlesRepository.AddAsync(article2);

            var article2Category3 = new ArticleCategory();
            article2Category3.Article = article2;
            article2Category3.Category = category3;

            await this.articleCategoriesRepository.AddAsync(article2Category3);

            var article3 = new Blog.Domain.Article();
            article3.Title = "title article 3";
            article3.Body = "body article 3";

            await this.articlesRepository.AddAsync(article3);

            var article3Category1 = new ArticleCategory();
            article3Category1.Article = article3;
            article3Category1.Category = category1;

            await this.articleCategoriesRepository.AddAsync(article3Category1);

            var actual = await this.articlesRetriever.GetCategoryArticlesAsync(category2.Id, null);

            Assert.Equal(2, actual.Count);
            Assert.Equal(article1.Id, actual[0].Id);
            Assert.Equal(article1.Title, actual[0].Title);
            Assert.Equal(article2.Id, actual[1].Id);
            Assert.Equal(article2.Title, actual[1].Title);
        }
    }
}
