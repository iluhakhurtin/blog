using System;
using System.Threading.Tasks;
using Blog.Domain;
using Npgsql;

namespace Blog.Repositories.PostgreSQL
{
    internal class ArticleRolesRepository : Repository<ArticleRole>, IArticleRolesRepository
    {
        public ArticleRolesRepository(string connectionString)
            : base(connectionString)
        {
        }

        public async Task AddAsync(ArticleRole articleRole)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = String.Format(@"
                    INSERT INTO ""ArticleRoles""(
                        ""ArticleId"",
	                    ""RoleId""
	                )
	                VALUES (
                        :ArticleId,
	                    :RoleId
	                );
                    ");

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue(":ArticleId", articleRole.ArticleId);
                    command.Parameters.AddWithValue(":RoleId", articleRole.RoleId);

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
                    DELETE FROM ""ArticleRoles""
                    WHERE ""ArticleId"" = :ArticleId
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
