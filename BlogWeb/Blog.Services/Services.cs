using System;
using Blog.Repositories;
using Blog.Retrievers;

namespace Blog.Services
{
    public interface IServices
    {
        IImagesService ImagesService { get; }
        ICategoriesService CategoriesService { get; }
    }
    
    public class Services : IServices
    {
        public IImagesService ImagesService { get; private set; }
        public ICategoriesService CategoriesService { get; private set; }

        public Services(IRepositories repositories, IRetrievers retrievers)
        {
            this.ImagesService = new ImagesService(repositories);
            this.CategoriesService = new CategoriesService(repositories);
        }
    }
}
