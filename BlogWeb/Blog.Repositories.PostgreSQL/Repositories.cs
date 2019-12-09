using System;
namespace Blog.Repositories.PostgreSQL
{
    public class Repositories : IRepositories
    {
        public IArticlesRepository ArticlesRepository { get; private set; }

        public Repositories(string blogConnectionString)
        {

        }
    }
}
