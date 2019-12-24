using System;
using Blog.Domain;
using Blog.Repositories;

namespace Blog.Services
{
    public interface IImagesService
    {
        File GetPreviewByOriginalFileName(string originalFileName);
    }

    public class ImagesService : IImagesService
    {
        private readonly IFilesRepository fileRepository;

        public ImagesService(IRepositories repositories)
        {
            this.fileRepository = repositories.FilesRepository;
        }

        public File GetPreviewByOriginalFileName(string originalFileName)
        {
            throw new NotImplementedException();
        }
    }
}
