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
            //var articleId = Guid.Parse(id);

            ////validate the roles
            //var roles = this.stringService.ParseCsvStringToArray(csvRoles);
            //if (roles != null)
            //{
            //    string errorMessage = await this.rolesService.ValidateRoles(roles);
            //    if(!String.IsNullOrEmpty(errorMessage))
            //        return errorMessage;
            //}

            //if (String.IsNullOrEmpty(title))
            //{
            //    return "Title cannot be empty.";
            //}

            //if (String.IsNullOrEmpty(body))
            //{
            //    return "Body cannot be empty.";
            //}

            //var article = await this.articlesRepository.GetAsync(articleId);

            //if (article.Title != title || article.Body != body)
            //{
            //    article.Title = title;
            //    article.Body = body;
            //    await this.articlesRepository.UpdateAsync(article);
            //}

            //await this.UpdateArticleRoles(article, roles);
            //await this.UpdateArticleCategories(article, csvCategories);

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
