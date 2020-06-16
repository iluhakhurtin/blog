using System;
using System.Threading.Tasks;
using Blog.Domain;

namespace Blog.Repositories
{
    public interface IGalleryRepository : IRepository<GalleryItem>
    {
        Task<GalleryItem> GetAsync(Guid id);
        Task AddAsync(GalleryItem galleryItem);
        Task UpdateAsync(GalleryItem galleryItem);
        Task DeleteAsync(Guid id);
    }
}
