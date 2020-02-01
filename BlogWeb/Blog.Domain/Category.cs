using System;
using System.Collections.Generic;

namespace Blog.Domain
{
    public class Category : IdEntity<Guid>
    {
        public virtual Guid? ParentId { get; set; }
        public virtual string Name { get; set; }

        #region Parent
        private Category parent;

        public virtual Category Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if(value == null)
                {
                    this.ParentId = null;
                }
                else
                {
                    this.ParentId = value.Id;
                }
                this.parent = value;
            }
        }

        #endregion

        public virtual IList<Category> Children { get; set; }

        public Category()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
