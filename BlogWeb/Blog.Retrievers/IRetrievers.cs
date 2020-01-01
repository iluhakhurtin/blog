﻿using System;
namespace Blog.Retrievers
{
    public interface IRetrievers
    {
        public IImagesRetriever ImagesRetriever { get; }
        public IArticlesRetriever ArticlesRetriever { get; }
    }
}
