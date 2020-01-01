using System;
namespace Blog.Retrievers.PostgreSQL
{
    public class Retrievers : IRetrievers
    {
        public IImagesRetriever ImagesRetriever { get; private set; }
        public IArticlesRetriever ArticlesRetriever { get; private set; }

        public Retrievers(string blogConnectionString)
        {
            this.ImagesRetriever = new ImagesRetriever(blogConnectionString);
            this.ArticlesRetriever = new ArticlesRetriever(blogConnectionString);
        }
    }
}
