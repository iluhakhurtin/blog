using System;
using Blog.Retrievers.Article;
using Blog.Retrievers.Image;
using Blog.Retrievers.PostgreSQL.Article;
using Blog.Retrievers.PostgreSQL.Image;
using Blog.Retrievers.PostgreSQL.User;
using Blog.Retrievers.User;

namespace Blog.Retrievers.PostgreSQL
{
    public class Retrievers : IRetrievers
    {
        public IImagesRetriever ImagesRetriever { get; private set; }
        public IArticlesRetriever ArticlesRetriever { get; private set; }
        public IUsersRetriever UsersRetriever { get; private set; }

        public Retrievers(string blogConnectionString)
        {
            this.ImagesRetriever = new ImagesRetriever(blogConnectionString);
            this.ArticlesRetriever = new ArticlesRetriever(blogConnectionString);
            this.UsersRetriever = new UsersRetriever(blogConnectionString);
        }
    }
}
