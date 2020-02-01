using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IArticleRolesRepository
    {
        Task AddAsync(ArticleRole articleRole);
    }
}
