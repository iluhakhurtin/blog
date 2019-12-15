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

        public async Task<Article> GetAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                ""Title"",
                                ""Body"",
                                ""Timestamp""
                                FROM ""Articles""
                                WHERE ""Id""=:Id;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var idParam = command.Parameters.AddWithValue("Id", id);
                    
                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        if (dataReader.Read())
                        {
                            var article = new Article();
                            article.Id = id;
                            article.Title = Convert.ToString(dataReader["Title"]);
                            article.Body = Convert.ToString(dataReader["Body"]);
                            article.Timestamp = Convert.ToDateTime(dataReader["Timestamp"]);

                            return article;
                        }
                    }
                }
            }
            return null;
        }
    }
}
