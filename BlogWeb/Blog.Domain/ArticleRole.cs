using System;
namespace Blog.Domain
{
    public class ArticleRole
    {
        public virtual Guid ArticleId { get; set; }
        public virtual Guid RoleId { get; set; }

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
                if(value != null)
                {
                    this.ArticleId = value.Id;
                }
                this.article = value;
            }
        }
        #endregion

        #region Role
        private ApplicationRole role;

        public ApplicationRole Role
        {
            get
            {
                return this.role;
            }
            set
            {
                if(value != null)
                {
                    this.RoleId = value.Id;
                }
                this.role = value;
            }
        }

        #endregion
    }
}
