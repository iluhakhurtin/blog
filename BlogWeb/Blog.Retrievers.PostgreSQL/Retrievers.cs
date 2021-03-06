﻿using System;
using Blog.Retrievers.Article;
using Blog.Retrievers.File;
using Blog.Retrievers.Gallery;
using Blog.Retrievers.Image;
using Blog.Retrievers.User;

namespace Blog.Retrievers.PostgreSQL
{
    public class Retrievers : IRetrievers
    {
        public IImagesRetriever ImagesRetriever { get; private set; }
        public IArticlesRetriever ArticlesRetriever { get; private set; }
        public IUsersRetriever UsersRetriever { get; private set; }
        public IFilesRetriever FilesRetriever { get; private set; }
        public IGalleryRetriever GalleryRetriever { get; private set; }

        public Retrievers(string blogConnectionString)
        {
            this.ImagesRetriever = new ImagesRetriever(blogConnectionString);
            this.ArticlesRetriever = new ArticlesRetriever(blogConnectionString);
            this.UsersRetriever = new UsersRetriever(blogConnectionString);
            this.FilesRetriever = new FilesRetriever(blogConnectionString);
            this.GalleryRetriever = new GalleryRetriever(blogConnectionString);
        }
    }
}
