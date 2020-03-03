using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Task<IList<Category>> GetAllAsync();
        Task<Category> GetAsync(Guid id);
        Task AddAsync(Category category);
    }
}
