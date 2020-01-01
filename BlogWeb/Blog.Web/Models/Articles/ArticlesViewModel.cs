using System;
using System.Collections.Generic;
using Blog.Retrievers;

namespace Blog.Web.Models.Articles
{
    public class ArticlesViewModel
    {
        public IList<IArticlesRetriever.ArticleDataResult> Articles { get; set; }

        public ArticlesViewModel()
        {

        }

        public ArticlesViewModel(IList<IArticlesRetriever.ArticleDataResult> articles)
        {
            if (articles == null)
                return;

            var articlesList = new List<IArticlesRetriever.ArticleDataResult>();
            articlesList.AddRange(articles);

            this.Articles = articlesList;
        }
    }
}
