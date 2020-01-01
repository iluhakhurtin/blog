using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class ArticlesRetriever : Retriever, IArticlesRetriever
    {
        public ArticlesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<IList<IArticlesRetriever.ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId)
        {
            var result = new List<IArticlesRetriever.ArticleDataResult>();

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                                ar.""Id"",
                                    ar.""Title""
                                    FROM ""ArticleCategories"" arc
                                    JOIN ""Articles"" ar ON ar.""Id"" = arc.""ArticleId""
                                    WHERE arc.""CategoryId"" = :CategoryId;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var imageIdParam = command.Parameters.AddWithValue("CategoryId", categoryId);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var item = new IArticlesRetriever.ArticleDataResult
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
