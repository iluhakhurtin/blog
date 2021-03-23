using System;
using System.Data;

namespace Blog.Retrievers.Article
{
    public class ArticleDataResultPagedItemsList : PagedItemsList<ArticleDataResult>
    {
        public ArticleDataResultPagedItemsList(int pageNumber, int pageSize)
            : base(pageNumber, pageSize)
        {
        }
    }
}
