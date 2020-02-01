using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IArticleRolesRepository : IRepository<ArticleRole>
    {
        Task AddAsync(ArticleRole articleRole);
    }
}
