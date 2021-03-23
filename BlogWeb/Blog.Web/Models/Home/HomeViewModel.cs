using System;
using System.Collections.Generic;
using Blog.Retrievers.Article;

namespace Blog.Web.Models.Home
{
    public class HomeViewModel : IPagerViewModel
    {
        #region IPaginationViewModel Props

        public int TotalPagesCount { get; set; }

        public int PageNumber { get; set; }

        #endregion

        public List<ArticleDataResult> Articles { get; set; }

        public HomeViewModel()
        {
            this.Articles = new List<ArticleDataResult>();
        }

        public HomeViewModel(ArticleDataResultPagedItemsList articleDataResultPagedItemsList)
            : this(articleDataResultPagedItemsList.Items)
        {
            this.TotalPagesCount = articleDataResultPagedItemsList.TotalPagesCount;
            this.PageNumber = articleDataResultPagedItemsList.PageNumber;
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
