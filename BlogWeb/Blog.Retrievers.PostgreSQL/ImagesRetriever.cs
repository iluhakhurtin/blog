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

        public async Task<dynamic> GetOriginalImageDataAsync(Guid imageId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                i.""MimeType"",
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
                            var result = new
                            {
                                MimeType = dataReader.GetString(0),
                                Data = await dataReader.GetStreamAsync(1)
                            };
                            return result;
                        }
                    }
                }
            }
            return null;
        }

        public Task<dynamic> GetPreviewImageDataAsync(Guid imageId)
        {
            throw new NotImplementedException();
        }
    }
}
