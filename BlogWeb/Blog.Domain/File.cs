using System;
namespace Blog.Domain
{
    public class File : IdEntity<Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Extension { get; set; }
        public virtual byte[] Data { get; set; }
        public virtual string MimeType { get; set; }
        public virtual DateTime Timestamp { get; set; }

        public File()
        {
            base.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
        }
    }
}
