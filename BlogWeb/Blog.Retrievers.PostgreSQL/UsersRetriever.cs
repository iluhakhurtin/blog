using System;
using System.Threading.Tasks;
using Blog.Retrievers.User;
using Npgsql;

namespace Blog.Retrievers.PostgreSQL
{
    internal class UsersRetriever : Retriever, IUsersRetriever
    {
        public UsersRetriever(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<UsersPagedDataTable> GetUsersPagedAsync(
            string emailFilter,
            string sortColumn,
            string sortOrder,
            int pageNumber,
            int pageSize)
        {
            UsersPagedDataTable usersPagedDataTable = new UsersPagedDataTable(pageNumber, pageSize);

            using (NpgsqlConnection connection = new NpgsqlConnection(base.connectionString))
            {
                string sql = @"SELECT
	                                *
	                                FROM
	                                (SELECT
		                                COUNT(1) OVER()                 AS ""ResultsCount"",
                                        u.""Id""                        AS ""Id"",
		                                MAX(u.""Email"")                AS ""Email"",
		                                string_agg(r.""Name"", ', ')    AS ""Roles""
		                                FROM ""AspNetUsers"" u
                                        LEFT JOIN ""AspNetUserRoles"" ur ON ur.""UserId"" = u.""Id""
                                        LEFT JOIN ""AspNetRoles"" r ON r.""Id"" = ur.""RoleId""
                                        WHERE(:EmailFilter IS NULL OR u.""Email"" ILIKE('%' || :EmailFilter || '%'))
                                        GROUP BY u.""Id"") Subquery
                                    ORDER BY
                                        CASE
                                            WHEN :SortOrder = 'desc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Email' THEN CAST(Subquery.""Email"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END DESC,
		                                CASE
                                            WHEN :SortOrder = 'asc' THEN
                                                CASE
                                                    WHEN :SortColumn = 'Email' THEN CAST(Subquery.""Email"" AS text)
					                                ELSE CAST(Subquery.""Id"" AS text)
				                                END
                                        END ASC
                                    LIMIT :PageSize
                                    OFFSET(:PageNumber - 1) * :PageSize;
                ";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    var emailFilterParam = new NpgsqlParameter<string>("EmailFilter", emailFilter);
                    command.Parameters.Add(emailFilterParam);

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
                            if (usersPagedDataTable.TotalResultsCount == 0)
                                usersPagedDataTable.TotalResultsCount = Convert.ToInt32(dataReader["ResultsCount"]);

                            usersPagedDataTable.Rows.Add(
                                dataReader[UsersPagedDataTable.Id],
                                dataReader[UsersPagedDataTable.Email],
                                dataReader[UsersPagedDataTable.Roles]);
                        }
                    }
                }
            }

            return usersPagedDataTable;
        }
    }
}
