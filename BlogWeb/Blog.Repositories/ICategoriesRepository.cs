using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface ICategoriesRepository : IRepository<Category>
    {
        Task<IList<Category>> GetAllAsync();
        Task AddAsync(Category category);
    }
}
