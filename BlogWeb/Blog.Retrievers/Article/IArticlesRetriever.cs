using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Retrievers.Article
{
    public interface IArticlesRetriever : IRetriever
    {
        Task<IList<ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId);
    }
}
