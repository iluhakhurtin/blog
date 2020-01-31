using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Retrievers.Image;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL.Image
{
    internal class ImagesRetriever : Retriever, IImagesRetriever
    {
        public ImagesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<ImageDataResult> GetOriginalImageDataAsync(Guid imageId)
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
                    command.Parameters.AddWithValue("ImageId", imageId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
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

        public async Task<ImageDataResult> GetPreviewImageDataAsync(Guid imageId)
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
                            var result = new ImageDataResult
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

        public async Task<ImageDataResult> GetPreviewImageDataByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                            prevf.""MimeType"",
                                prevf.""Data""
                                FROM ""Files"" f
                                JOIN ""Images"" i ON i.""OriginalFileId"" = f.""Id"" OR i.""PreviewFileId"" = f.""Id""
                                JOIN ""Files"" prevf ON prevf.""Id"" = i.""PreviewFileId""
                                WHERE f.""Name"" = :FileName
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var fileNameParam = command.Parameters.AddWithValue("FileName", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
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

        public async Task<ImageDataResult> GetOriginalImageDataByNameAsync(string fileName)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                            prevf.""MimeType"",
                                prevf.""Data""
                                FROM ""Files"" f
                                JOIN ""Images"" i ON i.""OriginalFileId"" = f.""Id"" OR i.""PreviewFileId"" = f.""Id""
                                JOIN ""Files"" prevf ON prevf.""Id"" = i.""OriginalFileId""
                                WHERE f.""Name"" = :FileName
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var fileNameParam = command.Parameters.AddWithValue("FileName", fileName);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var result = new ImageDataResult
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
