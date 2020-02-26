using System;
namespace Blog.Domain
{
    public class Image : IdEntity<Guid>
    {
        public virtual Guid PreviewFileId { get; set; }
        public virtual Guid OriginalFileId { get; set; }

        #region PreviewFile
        private File previewFile;

        public File PreviewFile
        {
            get
            {
                return this.previewFile;
            }
            set
            {
                if (value != null)
                    this.PreviewFileId = value.Id;
                this.previewFile = value;
            }
        }
        #endregion

        #region OriginalFile
        private File originalFile;

        public File OriginalFile
        {
            get
            {
                return this.originalFile;
            }
            set
            {
                if (value != null)
                    this.OriginalFileId = value.Id;
                this.originalFile = value;
            }
        }
        #endregion

        public Image()
        {
            base.Id = Guid.NewGuid();
        }
    }
}
