using System;
using System.Collections.Generic;
using Blog.Retrievers.Article;

namespace Blog.Web.Models.Home
{
    public class HomeViewModel
    {
        public List<ArticleDataResult> Articles { get; set; }

        public HomeViewModel()
        {
            this.Articles = new List<ArticleDataResult>();
        }

        public HomeViewModel(IList<ArticleDataResult> articles)
            : this()
        {
            if (articles == null)
                return;

            this.Articles.AddRange(articles);
        }
    }
}
