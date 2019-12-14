using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IArticlesRepository : IRepository<Article>
    {
        Task<Article> GetAsync(Guid id);
    }
}
