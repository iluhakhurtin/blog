using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blog.Domain;
using Blog.Repositories;
using Microsoft.AspNetCore.Http;

namespace Blog.Services
{
    public interface IFilesService
    {
        Task<string> AddFile(string name, string extension, string mimeType, IFormFile formFile);
        Task<string> EditFile(string id, string name, string extension, string mimeType);
    }


    public class FilesService: Service, IFilesService
    {
        private readonly IFilesRepository filesRepository;

        public FilesService(IRepositories repositories)
        {
            this.filesRepository = repositories.FilesRepository;
        }

        public async Task<string> AddFile(
            string name,
            string extension,
            string mimeType,
            IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return "File is empty";

            if (String.IsNullOrEmpty(name))
            {
                name = Path.GetFileName(formFile.FileName);
            }

            if (String.IsNullOrEmpty(extension))
            {
                extension = Path.GetExtension(formFile.FileName);
            }

            if (String.IsNullOrEmpty(mimeType))
            {
                mimeType = this.GetMimeType(extension);
            }

            var file = new Blog.Domain.File();
            file.Name = name;
            file.Extension = extension;
            file.MimeType = mimeType;

            using (var ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                file.Data = fileBytes;
            }

            await this.filesRepository.AddAsync(file);

            return String.Empty;
        }

        public async Task<string> EditFile(
            string id,
            string name,
            string extension,
            string mimeType)
        {
            var fileId = Guid.Parse(id);

            var file = await this.filesRepository.GetByIdAsync(fileId);

            if (file == null)
                return "File does not exist";

            if (!String.IsNullOrEmpty(name))
                file.Name = name;

            if (!String.IsNullOrEmpty(extension))
                file.Extension = extension;

            if (!String.IsNullOrEmpty(mimeType))
                file.MimeType = mimeType;

            await this.filesRepository.UpdateAsync(file);

            return String.Empty;
        }

        private string GetMimeType(string extension)
        {
            var ext = extension.ToLower().Trim();
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpg";
                case ".png":
                    return "image/png";
            }
            return " application/octet-stream";
        }
    }
}
