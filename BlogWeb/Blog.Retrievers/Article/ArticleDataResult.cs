using System;
namespace Blog.Retrievers.Article
{
    public class ArticleDataResult
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public Guid? CoverFileId { get; set; }
    }
}
