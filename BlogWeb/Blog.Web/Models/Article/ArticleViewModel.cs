using System;

namespace Blog.Web.Models.Article
{
    public class ArticleViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public ArticleViewModel()
        {

        }

        public ArticleViewModel(Blog.Domain.Article article)
        {
            this.Title = article.Title;
            this.Body = article.Body;
        }
    }
}
