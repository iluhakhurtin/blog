using System;
using Blog.Retrievers.Article;
using Blog.Retrievers.File;
using Blog.Retrievers.Gallery;
using Blog.Retrievers.Image;
using Blog.Retrievers.User;

namespace Blog.Retrievers
{
    public interface IRetrievers
    {
        public IImagesRetriever ImagesRetriever { get; }
        public IArticlesRetriever ArticlesRetriever { get; }
        public IUsersRetriever UsersRetriever { get; }
        public IFilesRetriever FilesRetriever { get; }
        public IGalleryRetriever GalleryRetriever { get; }
    }
}
