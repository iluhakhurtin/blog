using System;
using System.Collections.Generic;
using Blog.Retrievers;
using Blog.Retrievers.Article;

namespace Blog.Web.Models.Articles
{
    public class SearchArticlesViewModel
    {
        public String SearchPattern { get; set; }
        public List<ArticleDataResult> Articles { get; set; }

        public SearchArticlesViewModel()
        {
            this.SearchPattern = String.Empty;
            this.Articles = new List<ArticleDataResult>();
        }

        public SearchArticlesViewModel(IList<ArticleDataResult> articles) :
            this()
        {
            if (articles == null)
                return;

            this.Articles.AddRange(articles);
        }
    }
}
