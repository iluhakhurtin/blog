using System;
using System.IO;
using System.Threading.Tasks;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class ImagesRetriever : Retriever, IImagesRetriever
    {
        public ImagesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<IImagesRetriever.ImageDataResult> GetOriginalImageDataAsync(Guid imageId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                f.""MimeType"",
	                            f.""Data""
                                FROM ""Images"" i
                                JOIN ""Files"" f ON f.""Id"" = i.""OriginalFileId""
                                WHERE i.""Id"" = :ImageId;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var imageIdParam = command.Parameters.AddWithValue("ImageId", imageId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new IImagesRetriever.ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IImagesRetriever.ImageDataResult> GetPreviewImageDataAsync(Guid imageId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                f.""MimeType"",
	                            f.""Data""
                                FROM ""Images"" i
                                JOIN ""Files"" f ON f.""Id"" = i.""PreviewFileId""
                                WHERE i.""Id"" = :ImageId;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var imageIdParam = command.Parameters.AddWithValue("ImageId", imageId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new IImagesRetriever.ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IImagesRetriever.ImageDataResult> GetPreviewImageDataByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                            prevf.""MimeType"",
                                prevf.""Data""
                                FROM ""Files"" origf
                                JOIN ""Images"" i ON i.""OriginalFileId"" = origf.""Id""
                                JOIN ""Files"" prevf ON prevf.""Id"" = i.""PreviewFileId""
                                WHERE origf.""Name"" = :FileName
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var fileNameParam = command.Parameters.AddWithValue("FileName", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new IImagesRetriever.ImageDataResult
                            {
                                MimeType = (string)dataReader["MimeType"],
                                Data = (byte[])dataReader["Data"]
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}
