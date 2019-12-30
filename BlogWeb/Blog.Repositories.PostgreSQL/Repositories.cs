using System;
using System.Reflection;

namespace Blog.Repositories.PostgreSQL
{
    public class Repositories : IRepositories
    {
        public IArticlesRepository ArticlesRepository { get; private set; }
        public IFilesRepository FilesRepository { get; private set; }
        public ICategoriesRepository CategoriesRepository { get; private set; }

        public Repositories(string blogConnectionString)
        {
            this.ArticlesRepository = new ArticlesRepository(blogConnectionString);
            this.FilesRepository = new FilesRepository(blogConnectionString);
            this.CategoriesRepository = new CategoriesRepository(blogConnectionString);
        }
    }
}
