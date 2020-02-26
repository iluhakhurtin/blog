using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IImagesRepository : IRepository<Image>
    {
        Task<Image> GetAsync(Guid id);
        Task AddAsync(Image image);
        Task UpdateAsync(Image image);
        Task DeleteAsync(Guid id);
    }
}
