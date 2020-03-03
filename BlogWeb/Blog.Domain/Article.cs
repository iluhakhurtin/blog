using System;
namespace Blog.Domain
{
    public class Article : IdEntity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime Timestamp { get; set; }
        public virtual Guid? CoverFileId { get; set; }

        #region CoverFile

        private File coverFile;

        public File CoverFile
        {
            get
            {
                return this.coverFile;
            }
            set
            {
                if (value != null)
                {
                    this.CoverFileId = value.Id;
                }
                this.coverFile = value;
            }
        }

        #endregion

        public Article()
        {
            base.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
        }
    }
}
