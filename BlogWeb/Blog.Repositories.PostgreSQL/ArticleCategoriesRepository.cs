using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class ArticleCategoriesRepository : Repository<ArticleCategory>, IArticleCategoriesRepository
    {
        public ArticleCategoriesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task AddAsync(ArticleCategory articleCategory)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    INSERT INTO ""ArticleCategories""(
                        ""ArticleId"",
	                    ""CategoryId""
	                )
	                VALUES (
                        :ArticleId,
	                    :CategoryId
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":ArticleId", articleCategory.ArticleId);
                    command.Parameters.AddWithValue(":CategoryId", articleCategory.CategoryId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        public async Task DeleteAllForArticleAsync(Guid articleId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    DELETE FROM ""ArticleCategories""
                        WHERE ""ArticleId"" = :ArticleId;
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":ArticleId", articleId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
