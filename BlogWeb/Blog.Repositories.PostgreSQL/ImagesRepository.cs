using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class ImagesRepository : Repository<Image>, IImagesRepository
    {
        public ImagesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task AddAsync(Image image)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {

                string sql = String.Format(@"
                    INSERT INTO ""Images""(
                        ""Id"",
	                    ""PreviewFileId"",
	                    ""OriginalFileId""
	                )
	                VALUES (
                        :Id,
	                    :PreviewFileId,
	                    :OriginalFileId
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", image.Id);
                    command.Parameters.AddWithValue(":OriginalFileId", image.OriginalFileId);
                    command.Parameters.AddWithValue(":PreviewFileId", image.PreviewFileId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task UpdateAsync(Image image)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    UPDATE ""Images"" SET
	                    ""PreviewFileId"" = :PreviewFileId, 
	                    ""OriginalFileId"" = :OriginalFileId
                    WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", image.Id);
                    command.Parameters.AddWithValue(":PreviewFileId", image.PreviewFileId);
                    command.Parameters.AddWithValue(":OriginalFileId", image.OriginalFileId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task<Image> GetAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    SELECT
                        ""PreviewFileId"",
                        ""OriginalFileId""
                        FROM ""Images""
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
                            var image = new Image();
                            image.Id = id;
                            image.PreviewFileId = (Guid)dataReader["PreviewFileId"];
                            image.PreviewFileId = (Guid)dataReader["OriginalFileId"];

                            return image;
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
                    DELETE FROM ""Images""
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
