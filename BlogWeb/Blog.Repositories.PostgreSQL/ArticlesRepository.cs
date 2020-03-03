using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class ArticlesRepository : Repository<Article>, IArticlesRepository
    {
        public ArticlesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task AddAsync(Article article)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    INSERT INTO ""Articles""(
                        ""Id"",
	                    ""Title"", 
	                    ""Body"",
                        ""Timestamp"",
                        ""CoverFileId""
	                )
	                VALUES (
                        :Id,
	                    :Title, 
	                    :Body,
                        :Timestamp,
                        :CoverFileId
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", article.Id);
                    command.Parameters.AddWithValue(":Title", article.Title);
                    command.Parameters.AddWithValue(":Body", article.Body);
                    command.Parameters.AddWithValue(":Timestamp", article.Timestamp);
                    command.Parameters.AddWithValue(":CoverFileId", article.CoverFileId ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task<Article> GetAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                ""Title"",
                                ""Body"",
                                ""Timestamp"",
                                ""CoverFileId""
                                FROM ""Articles""
                                WHERE ""Id""=:Id;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var idParam = command.Parameters.AddWithValue("Id", id);
                    
                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (await dataReader.ReadAsync())
                        {
                            var article = new Article();
                            article.Id = id;
                            article.Title = Convert.ToString(dataReader["Title"]);
                            article.Body = Convert.ToString(dataReader["Body"]);
                            article.Timestamp = Convert.ToDateTime(dataReader["Timestamp"]);

                            var coverFileId = dataReader["CoverFileId"];
                            if(coverFileId != DBNull.Value)
                                article.CoverFileId = (Guid)coverFileId;

                            return article;
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
                    DELETE FROM ""Articles""
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

        public async Task UpdateAsync(Article article)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    UPDATE ""Articles"" SET
	                    ""Title"" = :Title, 
	                    ""Body"" = :Body,
                        ""CoverFileId"" = :CoverFileId
                    WHERE ""Id"" = :Id;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":Id", article.Id);
                    command.Parameters.AddWithValue(":Title", article.Title);
                    command.Parameters.AddWithValue(":Body", article.Body);
                    command.Parameters.AddWithValue(":CoverFileId", article.CoverFileId ?? (object)DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
