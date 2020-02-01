using System;
namespace Blog.Domain
{
    public class Article : IdEntity<Guid>
    {
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual DateTime Timestamp { get; set; }

        public Article()
        {
            base.Id = Guid.NewGuid();
            this.Timestamp = DateTime.Now;
        }
    }
}
