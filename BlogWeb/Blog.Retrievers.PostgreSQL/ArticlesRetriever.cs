using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Retrievers.Article;
using Npgsql;
using Blog.Domain;

namespace Blog.Retrievers.PostgreSQL
{
    internal class ArticlesRetriever : Retriever, IArticlesRetriever
    {
        public ArticlesRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<ArticlesPagedDataTable> GetArticlesPagedAsync(
            string titleFilter,
            string rolesFilter,
            string categoriesFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize)
        {
            var articlesPagedDataTable = new ArticlesPagedDataTable(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                                *
	                                FROM
	                                (SELECT
		                                COUNT(1) OVER()                         AS ""ResultsCount"",
                                        a.""Id""                                AS ""Id"",
                                        a.""Title""                             AS ""Title"",
		                                a.""Timestamp""                         AS ""Timestamp"",
                                        a.""CoverFileId""                       AS ""CoverFileId"",
		                                string_agg(DISTINCT r.""Name"", ', ')   AS ""Roles"",
	 	                                string_agg(DISTINCT c.""Name"", ', ')   AS ""Categories""
                                        FROM ""Articles"" a
                                        LEFT JOIN ""ArticleRoles"" ar ON ar.""ArticleId"" = a.""Id""
                                        LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ar.""RoleId""
                                        LEFT JOIN ""ArticleCategories"" ac ON ac.""ArticleId"" = a.""Id""
                                        LEFT JOIN ""Categories"" c ON c.""Id"" = ac.""CategoryId""
                                        WHERE :TitleFilter IS NULL OR a.""Title"" ILIKE('%' || :TitleFilter || '%')
                                        GROUP BY a.""Id"", a.""Title"", a.""Timestamp"", a.""CoverFileId"") Subquery
                                    WHERE :RolesFilter IS NULL OR Subquery.""Roles"" ILIKE('%' || :RolesFilter || '%')
                                        AND :CategoriesFilter IS NULL OR Subquery.""Categories"" ILIKE ('%' || :CategoriesFilter || '%')
                                    ORDER BY
                                        CASE
                                            WHEN :SortOrder = 'desc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Title' THEN CAST(Subquery.""Title"" AS text)
                                                    WHEN :SortColumn = 'Timestamp' THEN CAST(Subquery.""Timestamp"" AS text)
                                                    WHEN :SortColumn = 'Roles' THEN CAST(Subquery.""Roles"" AS text)
                                                    WHEN :SortColumn = 'Categories' THEN CAST(Subquery.""Categories"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END DESC,
		                                CASE
                                            WHEN :SortOrder = 'asc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Title' THEN CAST(Subquery.""Title"" AS text)
                                                    WHEN :SortColumn = 'Timestamp' THEN CAST(Subquery.""Timestamp"" AS text)
                                                    WHEN :SortColumn = 'Roles' THEN CAST(Subquery.""Roles"" AS text)
                                                    WHEN :SortColumn = 'Categories' THEN CAST(Subquery.""Categories"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END ASC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                                    ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var filterParam = new NpgsqlParameter<string>("TitleFilter", titleFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("RolesFilter", rolesFilter);
                    command.Parameters.Add(filterParam);

                    filterParam = new NpgsqlParameter<string>("CategoriesFilter", categoriesFilter);
                    command.Parameters.Add(filterParam);

                    var sortColumnParam = new NpgsqlParameter<string>("SortColumn", sortColumn);
                    command.Parameters.Add(sortColumnParam);

                    command.Parameters.AddWithValue("SortOrder", sortOrder);
                    command.Parameters.AddWithValue("PageNumber", pageNumber);
                    command.Parameters.AddWithValue("PageSize", pageSize);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (dataReader.Read())
                        {
                            if (articlesPagedDataTable.TotalResultsCount == 0)
                                articlesPagedDataTable.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            var timestamp = dataReader[ArticlesPagedDataTable.Timestamp];
                            var timestampStr = timestamp == null ? String.Empty : ((DateTime)timestamp).ToShortDateString();

                            articlesPagedDataTable.Rows.Add(
                                dataReader[ArticlesPagedDataTable.Id],
                                dataReader[ArticlesPagedDataTable.CoverFileId],
                                dataReader[ArticlesPagedDataTable.Title],
                                timestampStr,
                                dataReader[ArticlesPagedDataTable.Roles],
                                dataReader[ArticlesPagedDataTable.Categories]);
                        }
                    }
                }
            }

            return articlesPagedDataTable;
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

        public async Task<IList<ArticleDataResult>> GetCategoryArticlesAsync(Guid categoryId, IEnumerable<string> roles)
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
                                    LEFT JOIN ""ArticleRoles"" ar ON ar.""ArticleId"" = a.""Id""
                                    LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ar.""RoleId""
                                    WHERE r.""Name"" IS NULL OR r.""Name"" = ANY(:Roles)
                                    ORDER BY a.""Timestamp"", a.""Title""
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("CategoryId", categoryId);

                    var rolesParam = new NpgsqlParameter("Roles", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text);
                    rolesParam.Value = roles == null ? (object)DBNull.Value : (object)roles;
                    command.Parameters.Add(rolesParam);

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

        public async Task<IList<ArticleDataResult>> FindArticlesAsync(string searchPattern, IEnumerable<string> roles)
        {
            var result = new List<ArticleDataResult>();

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"WITH RECURSIVE ""RecursiveCategories"" AS (
                                    SELECT
                                        ""Id"",
		                                ""ParentId"",
		                                ""Name""
                                        FROM ""Categories""
                                    UNION
                                    SELECT
                                        c.""Id"",
		                                c.""ParentId"",
		                                c.""Name""
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
                                    LEFT JOIN ""ArticleRoles"" ar ON ar.""ArticleId"" = a.""Id""
                                    LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ar.""RoleId""
                                    WHERE (
                                        (a.""Title"" ILIKE('%' || :SearchPattern || '%'))
                                        OR (a.""Body"" ILIKE('%' || :SearchPattern || '%'))
                                        OR (rc.""Name"" ILIKE('%' || :SearchPattern || '%'))
                                    )
                                    AND (r.""Name"" IS NULL OR r.""Name"" = ANY(:Roles))
                                    ORDER BY a.""Timestamp"", a.""Title""
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("SearchPattern", searchPattern);

                    var rolesParam = new NpgsqlParameter("Roles", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text);
                    rolesParam.Value = roles;
                    command.Parameters.Add(rolesParam);

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

        public async Task<ArticleDataResultPagedItemsList> GetLatestArticleItemsPagedAsync(int pageNumber, int pageSize, IEnumerable<string> roles)
        {
            var articleDataResultPagedItemsList = new ArticleDataResultPagedItemsList(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
                                    COUNT(1) OVER()		AS ""ResultsCount"",
                                    a.""Id"",
                                    a.""Title"",
                                    a.""CoverFileId""
                                    FROM ""Articles"" a
                                    LEFT JOIN ""ArticleRoles"" ar ON ar.""ArticleId"" = a.""Id""
                                    LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ar.""RoleId""
                                    WHERE r.""Name"" IS NULL OR r.""Name"" = ANY(:Roles)
                                    ORDER BY ""Timestamp"" DESC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                            ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("PageNumber", pageNumber);
                    command.Parameters.AddWithValue("PageSize", pageSize);

                    var rolesParam = new NpgsqlParameter("Roles", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Text);
                    rolesParam.Value = roles;
                    command.Parameters.Add(rolesParam);

                    await connection.OpenAsync();

                    using (NpgsqlDataReader dataReader = await command.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            if (articleDataResultPagedItemsList.TotalResultsCount == 0)
                                articleDataResultPagedItemsList.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            var item = new ArticleDataResult(dataReader);
                            articleDataResultPagedItemsList.AddItem(item);
                        }
                    }
                }
            }

            return articleDataResultPagedItemsList;
        }
    }
}
