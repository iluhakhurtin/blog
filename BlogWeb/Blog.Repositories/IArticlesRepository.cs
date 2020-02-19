using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IArticlesRepository : IRepository<Article>
    {
        Task AddAsync(Article article);
        Task<Article> GetAsync(Guid id);
        Task UpdateAsync(Article article);
        Task DeleteAsync(Guid id);
    }
}
