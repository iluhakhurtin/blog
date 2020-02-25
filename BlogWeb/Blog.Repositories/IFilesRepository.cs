using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IFilesRepository : IRepository<File>
    {
        Task AddAsync(File file);
        Task UpdateAsync(File file);
        Task DeleteAsync(Guid id);
        Task<File> GetByNameAsync(string fileName);
        Task<File> GetByIdAsync(Guid fileId);
    }
}
