using System;
using Blog.Domain;
using Blog.Repositories;
using Blog.Retrievers;
using Microsoft.AspNetCore.Identity;

namespace Blog.Services
{
    public interface IServices
    {
        IStringService StringService { get; }
        IRolesService RolesService { get; }
        IImagesService ImagesService { get; }
        ICategoriesService CategoriesService { get; }
        IArticlesService ArticlesService { get; }
    }
    
    public class Services : IServices
    {
        public IStringService StringService { get; private set; }
        public IRolesService RolesService { get; private set; }
        public IImagesService ImagesService { get; private set; }
        public ICategoriesService CategoriesService { get; private set; }
        public IArticlesService ArticlesService { get; private set; }

        public Services(IRepositories repositories,
            IRetrievers retrievers,
            RoleManager<ApplicationRole> roleManager)
        {
            this.StringService = new StringService();
            this.RolesService = new RolesService(roleManager);
            this.ImagesService = new ImagesService(repositories);
            this.CategoriesService = new CategoriesService(repositories);
        }
    }
}
