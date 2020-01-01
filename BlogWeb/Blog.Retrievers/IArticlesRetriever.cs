using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Retrievers
{
    public interface IArticlesRetriever : IRetriever
    {
        Task<IList<ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId);

        public class ArticleDataResult
        {
            public Guid Id { get; set; }
            public String Title { get; set; }
        }
    }
}
