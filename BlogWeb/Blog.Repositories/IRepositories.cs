using System;
namespace Blog.Repositories
{
    public interface IRepositories
    {
        public IArticlesRepository ArticlesRepository { get; }
    }
}
