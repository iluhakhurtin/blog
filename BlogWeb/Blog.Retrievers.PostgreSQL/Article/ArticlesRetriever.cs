using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Retrievers.Article;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL.Article
{
    internal class ArticlesRetriever : Retriever, IArticlesRetriever
    {
        public ArticlesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<ArticleWithRolesDataResult> GetArticleWithRolesAsync(Guid articleId)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                    a.""Id"",
                                    a.""Title"",
	                                a.""Body"",
	                                a.""Timestamp"",
	                                string_agg(r.""Name"", ', ') AS ""Roles""
                                    FROM ""Articles"" a
                                    LEFT JOIN ""ArticleRoles"" ar ON ar.""ArticleId"" = a.""Id""
                                    LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ar.""RoleId""
                                    WHERE a.""Id"" = :ArticleId
                                    GROUP BY a.""Id"", a.""Title"", a.""Body"", a.""Timestamp"";
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("ArticleId", articleId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var articleWithRolesDataResult = new ArticleWithRolesDataResult
                            {
                                Id = (Guid)dataReader["Id"],
                                Title = (string)dataReader["Title"],
                                Body = (string)dataReader["Body"],
                                Timestamp = (DateTime)dataReader["Timestamp"],
                                Roles = Convert.ToString(dataReader["Roles"])
                            };

                            return articleWithRolesDataResult;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<IList<ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId)
        {
            var result = new List<ArticleDataResult>();

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"WITH RECURSIVE ""RecursiveCategories"" AS (
                                    SELECT
                                        ""Id"",
                                        ""ParentId""
                                        FROM ""Categories""
                                        WHERE ""Id"" = :CategoryId
                                    UNION
                                    SELECT
                                        c.""Id"",
                                        c.""ParentId""
                                        FROM ""Categories"" c
                                        JOIN ""RecursiveCategories"" rc ON rc.""Id"" = c.""ParentId""
                                )
                                SELECT
                                    DISTINCT
                                        a.""Id"",
                                        a.""Title"",
                                        a.""Timestamp""
                                    FROM ""RecursiveCategories"" rc
                                    JOIN ""ArticleCategories"" ac ON ac.""CategoryId"" = rc.""Id""
                                    JOIN ""Articles"" a ON a.""Id"" = ac.""ArticleId""
                                    ORDER BY a.""Timestamp"", a.""Title""
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("CategoryId", categoryId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var item = new ArticleDataResult
                            {
                                Id = (Guid)dataReader["Id"],
                                Title = (string)dataReader["Title"]
                            };

                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }
    }
}
