using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IArticleCategoriesRepository : IRepository<ArticleCategory>
    {
        Task AddAsync(ArticleCategory articleCategory);
    }
}
