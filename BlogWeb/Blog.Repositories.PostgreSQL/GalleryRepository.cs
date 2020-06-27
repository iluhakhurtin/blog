using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class GalleryRepository : Repository<GalleryItem>, IGalleryRepository
    {
        public GalleryRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task AddAsync(GalleryItem galleryItem)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {

                string sql = String.Format(@"
                    INSERT INTO ""Gallery""(
                        ""Id"",
	                    ""ImageId"",
                        ""SmallPreviewFileId"",
                        ""ArticleId"",
                        ""Title"",
                        ""Description"",
                        ""Timestamp""
                    )
	                VALUES (
                        :Id,
	                    :ImageId,
	                    :SmallPreviewFileId,
                        :ArticleId,
                        :Title,
                        :Description,
                        :Timestamp
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", galleryItem.Id);
                    command.Parameters.AddWithValue(":ImageId", galleryItem.ImageId);
                    command.Parameters.AddWithValue(":SmallPreviewFileId", galleryItem.SmallPreviewFileId);
                    command.Parameters.AddWithValue(":ArticleId", galleryItem.ArticleId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue(":Title", galleryItem.Title);
                    command.Parameters.AddWithValue(":Description", galleryItem.Description);

                    var timestamp = command.CreateParameter();
                    timestamp.Direction = System.Data.ParameterDirection.Input;
                    timestamp.DbType = System.Data.DbType.DateTime;
                    timestamp.ParameterName = ":Timestamp";
                    timestamp.Value = galleryItem.Timestamp;
                    command.Parameters.Add(timestamp);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task UpdateAsync(GalleryItem galleryItem)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    UPDATE ""Gallery"" SET
                        ""ImageId"" = :ImageId,
	                    ""SmallPreviewFileId"" = :SmallPreviewFileId, 
	                    ""ArticleId"" = :ArticleId,
                        ""Title"" = :Title,
                        ""Description"" = :Description,
                        ""Timestamp"" = :Timestamp
                    WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", galleryItem.Id);
                    command.Parameters.AddWithValue(":ImageId", galleryItem.ImageId);
                    command.Parameters.AddWithValue(":SmallPreviewFileId", galleryItem.SmallPreviewFileId);
                    command.Parameters.AddWithValue(":ArticleId", galleryItem.ArticleId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue(":Title", galleryItem.Title);
                    command.Parameters.AddWithValue(":Description", galleryItem.Description);

                    var timestamp = command.CreateParameter();
                    timestamp.Direction = System.Data.ParameterDirection.Input;
                    timestamp.DbType = System.Data.DbType.DateTime;
                    timestamp.ParameterName = ":Timestamp";
                    timestamp.Value = galleryItem.Timestamp;
                    command.Parameters.Add(timestamp);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task<GalleryItem> GetAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    SELECT
                        ""Id"",
                        ""ImageId"",
                        ""SmallPreviewFileId"",
                        ""ArticleId"",
                        ""Title"",
                        ""Description"",
                        ""Timestamp""
                        FROM ""Gallery""
                        WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("Id", id);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var galleryItem = new GalleryItem(dataReader);
                            return galleryItem;
                        }
                    }
                }
            }
            return null;
        }

        public async Task DeleteAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    DELETE FROM ""Gallery""
                        WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
