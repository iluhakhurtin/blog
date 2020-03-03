using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Retrievers.Article
{
    public interface IArticlesRetriever : IRetriever
    {
        Task<IList<ArticleDataResult>> GetLatestArticles(int count);
        Task<IList<ArticleDataResult>> FindArticlesAsync(string searchPattern);
        Task<IList<ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId);
        Task<ArticleWithRolesDataResult> GetArticleWithRolesAsync(Guid articleId);
        Task<ArticlesPagedDataTable> GetArticlesPagedAsync(
            string titleFilter,
            string rolesFilter,
            string categoriesFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize);
    }
}
