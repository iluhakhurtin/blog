using System;
namespace Blog.Repositories
{
    public interface IRepositories
    {
        public IArticlesRepository ArticlesRepository { get; }
        public IFilesRepository FilesRepository { get; }
        public ICategoriesRepository CategoriesRepository { get; }
        public IArticleRolesRepository ArticleRolesRepository { get; }
    }
}
