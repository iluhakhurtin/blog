using System;
using System.Collections.Generic;
using Blog.Retrievers;
using Blog.Retrievers.Article;

namespace Blog.Web.Models.Articles
{
    public class ArticlesViewModel
    {
        public String SearchPattern { get; set; }
        public IList<ArticleDataResult> Articles { get; set; }

        public ArticlesViewModel()
        {

        }

        public ArticlesViewModel(IList<ArticleDataResult> articles)
        {
            if (articles == null)
                return;

            var articlesList = new List<ArticleDataResult>();
            articlesList.AddRange(articles);

            this.Articles = articlesList;
        }
    }
}
