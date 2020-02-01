using System;
namespace Blog.Domain
{
    public class ArticleCategory : Entity
    {
        public virtual Guid ArticleId { get; set; }
        public virtual Guid CategoryId { get; set; }

        #region Article
        private Article article;

        public Article Article
        {
            get
            {
                return this.article;
            }
            set
            {
                if (value != null)
                    this.ArticleId = value.Id;
                this.article = value;
            }
        }
        #endregion

        #region Category
        private Category category;

        public Category Category
        {
            get
            {
                return this.category;
            }
            set
            {
                if (value != null)
                    this.CategoryId = value.Id;
                this.category = value;
            }
        }
        #endregion

        public ArticleCategory()
        {
        }
    }
}
