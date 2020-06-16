using System;
namespace Blog.Domain
{
    public class GalleryItem : IdEntity<Guid>
    {
        public virtual Guid SmallPreviewFileId { get; set; }
        public virtual Guid ImageId { get; set; }
        public virtual Guid? ArticleId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime Timestamp { get; set; }

        #region Image
        private Image image;

        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                if (value != null)
                    this.ImageId = value.Id;
                this.image = value;
            }
        }
        #endregion

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

        public GalleryItem()
        {
            base.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
        }
    }
}
