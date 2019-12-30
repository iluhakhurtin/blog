using System;
using System.Collections.Generic;

namespace Blog.Domain
{
    public class Category : IdEntity<Guid>
    {
        public virtual Guid? ParentId { get; set; }
        public virtual string Name { get; set; }

        public virtual Category Parent { get; set; }
        public virtual IList<Category> Children { get; set; }

        public Category()
        {
        }
    }
}
