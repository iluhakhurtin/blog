using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IFilesRepository : IRepository<File>
    {
        Task<File> GetByNameAsync(string fileName);
    }
}
