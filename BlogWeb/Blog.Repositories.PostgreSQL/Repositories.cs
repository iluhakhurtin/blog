using System;
using System.Reflection;

namespace Blog.Repositories.PostgreSQL
{
    public class Repositories : IRepositories
    {
        public IArticlesRepository ArticlesRepository { get; private set; }
        
        public Repositories(string blogConnectionString)
        {
            this.ArticlesRepository = new ArticlesRepository(blogConnectionString);
        }
    }
}
