using System;
namespace Blog.Domain
{
    public interface IHaveId<T>
    {
        public T Id { get; set; }
    }
}
