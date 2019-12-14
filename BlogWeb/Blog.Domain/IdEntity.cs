using System;
namespace Blog.Domain
{
    public abstract class IdEntity<T> : IHaveId<T>
    {
        public T Id { get; set; }
    }
}
